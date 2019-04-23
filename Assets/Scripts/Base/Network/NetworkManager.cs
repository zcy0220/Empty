/**
 * 网络管理
 */

using System;
using System.Net;
using Base.Debug;
using Base.Utils;
using Base.Common;
using Base.Network;
using Base.Collections;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;

public class NetworkManager : MonoSingleton<NetworkManager>
{
    /// <summary>
    /// 消息队列最大容量
    /// </summary>
    const int MAX_MSG_QUEUECOUNT = 10;
    /// <summary>
    /// 接收消息的最大容量
    /// </summary>
    const int MAX_RECEIVE_SIZE = 1024;
    #region Socket
    private string mHost;
    private int mPort;
    private Socket mSocket;
    private ESocketState mSocketState = ESocketState.CLOSED;
    #endregion
    #region Send
    private bool mSendWork;
    private Thread mSendThread;
    private object mSendLock = new object();
    private CircularQueue<byte[]> mSendMsgQueue = new CircularQueue<byte[]>(MAX_MSG_QUEUECOUNT);
    #endregion
    #region Receive
    private bool mReceiveWork;
    private Thread mReceiveThread;
    private object mReceiveLock = new object();
    private CircularQueue<byte[]> mReceiveMsgQueue = new CircularQueue<byte[]>(MAX_MSG_QUEUECOUNT);
    #endregion
    //============================================================================================
    public bool Connected { get { return mSocketState == ESocketState.CONNECTED; } }

    /// <summary>
    /// 根据主机和端口连接
    /// </summary>
    public void Connect(string host, int port)
    {
        mHost = host;
        mPort = port;
        Connect();
    }

    /// <summary>
    /// 连接
    /// </summary>
    public void Connect()
    {
        Close();
        try
        {
            // 兼容IPV6
            var address = Dns.GetHostAddresses(mHost);
            mSocket = new Socket(address[0].AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            mSocketState = ESocketState.CONNECTING;
            mSocket.BeginConnect(mHost, mPort, (IAsyncResult ar) =>
            {
                mSocket.EndConnect(ar);
                OnConnected();
            }, null);
        }
        catch (Exception e)
        {
            Debugger.LogError(e.Message);
        }
    }

    /// <summary>
    /// 连接成功
    /// </summary>
    private void OnConnected()
    {
        Debugger.Log("连接成功");
        mSocketState = ESocketState.CONNECTED;
        StartAllThead();
        // todo 通知上层连接成功
    }
    
    /// <summary>
    /// 关闭Socket
    /// </summary>
    public void Close()
    {
        if (mSocket == null) return;
        mSocketState = ESocketState.CLOSED;
        try
        {
            mSocket.Close();
            mSocket = null;
            StopAllThread();
        }
        catch (Exception e)
        {
            Debugger.Log(e.Message);
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    public void Send(byte[] buffer)
    {
        lock (mSendLock)
        {
            mSendMsgQueue.Enqueue(buffer);
        }
    }

    /// <summary>
    /// 根据协议id发送 兼顾Lua使用
    /// </summary>
    private void Send(int msgId, byte[] msgBody)
    {
        // 总长度 = 数据包长度所占字节 + 协议号长度 + 协议内容长度
        var intSize = sizeof(int); // 可以直接写死
        var totalSize = intSize + intSize + msgBody.Length;
        // 为了书写方便而写的中间字节buffer类，其实可以暴力点直接处理byte[]还能省去这步
        var byteBuffer = new ByteBuffer(totalSize);
        // 写入数据长度
        byteBuffer.WriteInt(totalSize - intSize);
        // 写入协议号
        byteBuffer.WriteInt(msgId);
        // 写入协议内容
        byteBuffer.WriteBytes(msgBody);
        Send(byteBuffer.GetBytes()); 
    }

    /// <summary>
    /// 发送协议消息 C#使用
    /// </summary>
    public void Send<T>(int msgId, T request)
    {
        var buffer = ProtobufUtil.NSerialize(request);
        Send(msgId, buffer);
    }

    /// <summary>
    /// 开启所有线程
    /// </summary>
    private void StartAllThead()
    {
        if (mSendThread == null)
        {
            mSendWork = true;
            mSendThread = new Thread(SendThread);
            mSendThread.Start(); 
        }

        if (mReceiveThread == null)
        {
            mReceiveWork = true;
            mReceiveThread = new Thread(ReceiveThead);
            mReceiveThread.Start();
        }
    }
    
    /// <summary>
    /// 发送线程执行方法
    /// </summary>
    private void SendThread()
    {
        while (mSendWork)
        {
            if (!mSendMsgQueue.IsEmpty)
            {
                lock (mSendLock)
                {
                    var buffer = mSendMsgQueue.Dequeue();
                    mSocket.Send(buffer, buffer.Length, SocketFlags.None);
                }
            }
        }
    }

    /// <summary>
    /// 接收线程执行方法
    /// </summary>
    private void ReceiveThead()
    {
        var byteBuffer = ByteBufferPool.Instance.Spawn(MAX_RECEIVE_SIZE);
        var curBufferLength = 0;
        while (mReceiveWork)
        {
            try
            {
                var leftBufferLength = byteBuffer.Capacity - curBufferLength;
                var readLength = mSocket.Receive(byteBuffer.GetBytes(), curBufferLength, leftBufferLength, SocketFlags.None);
                if (readLength > 0)
                {
                    curBufferLength += readLength;
                    DoReceive(byteBuffer, ref curBufferLength);
                }
            }
            catch (Exception e)
            {
                Debugger.LogError(e.Message);
                break;
            }
        }
        ByteBufferPool.Instance.Recycle(byteBuffer);
    }

    /// <summary>
    /// 拆包组包
    /// </summary>
    private void DoReceive(ByteBuffer byteBuffer, ref int curBufferLength)
    {
        try
        {
            var data = byteBuffer.GetBytes();
            var intSize = sizeof(int);
            // 数据包长度所占字节是否达到
            if (curBufferLength < intSize) return;
            var msgLength = byteBuffer.ReadInt(0);
            // 数据长度是否达到
            if (curBufferLength < intSize + msgLength) return;
            var recvBuffer = new byte[msgLength];
            Buffer.BlockCopy(data, intSize, recvBuffer, 0, msgLength);
            curBufferLength -= (intSize + msgLength);
            // 把后面的字节拷到前面
            Buffer.BlockCopy(data, intSize + msgLength, data, 0, curBufferLength);
            lock (mReceiveLock)
            {
                mReceiveMsgQueue.Enqueue(recvBuffer);
            }
        }
        catch (Exception e)
        {
            Debugger.LogError("Socket receive package err: {0}\n{1}", e.Message, e.StackTrace);
        }
    }

    /// <summary>
    /// 处理接收的消息
    /// </summary>
    private void HandleRecvMsg()
    {
        if (!mReceiveMsgQueue.IsEmpty)
        {
            lock (mReceiveLock)
            {
                var data = mReceiveMsgQueue.Dequeue();
                // BitConverter.ToInt32(data, 0) 有大端和小端问题，不如直接手动转
                var msgId = BytesUtil.ReadInt(data, 0);
                // NetMsg里定义了协议Id和协议类型的映射，所有比较耦合
                var type = NetMsg.GetTypeByMsgId(msgId);
                var response = ProtobufUtil.NDeserialize(type, data, sizeof(int));
                Debugger.Log(msgId);
                Debugger.Log(response);
                HandlerEventListener(msgId, response);
            }
        }
    }

    /// <summary>
    /// 停止所有线程
    /// </summary>
    public void StopAllThread()
    {
        mSendMsgQueue.Clear();
        mReceiveMsgQueue.Clear();
        if (mSendThread != null)
        {
            mSendWork = false;
            mSendThread.Join();
            mSendThread = null;
        }
        
        if (mReceiveThread != null)
        {
            mReceiveWork = false;
            mReceiveThread.Join();
            mReceiveThread = null;
        }
    }

    /// <summary>
    /// 主线程Update处理UI相关
    /// </summary>
    private void Update()
    {
        HandleRecvMsg();
    }

    #region NetMsgEventListener
    /// <summary>
    /// 回调委托
    /// </summary>
    public delegate void OnResponse(object obj);
    /// <summary>
    /// 网络消息回调处理
    /// </summary>
    private Dictionary<int, List<OnResponse>> mResponseListDict = new Dictionary<int, List<OnResponse>>();

    /// <summary>
    /// 添加协议监听
    /// </summary>
    public void AddNetMsgEventListener(int msgId, OnResponse callback)
    {
        if (callback == null) return;
        if (!mResponseListDict.ContainsKey(msgId))
        {
            mResponseListDict.Add(msgId, new List<OnResponse>());
        }
        var responseList = mResponseListDict[msgId];
        if (!responseList.Contains(callback)) responseList.Add(callback);
    }

    /// <summary>
    /// 删除协议监听
    /// </summary>
    public void RemoveNetMsgEventListener(int msgId, OnResponse callback)
    {
        if (mResponseListDict.ContainsKey(msgId))
        {
            var responseList = mResponseListDict[msgId];
            responseList.Remove(callback);
        }
    }

    /// <summary>
    /// 处理回调
    /// </summary>
    private void HandlerEventListener(int msgId, object arg)
    {
        if (mResponseListDict.ContainsKey(msgId))
        {
            var responseList = mResponseListDict[msgId];
            foreach (var handler in responseList)
            {
                handler(arg);
            }
        }
    }
    #endregion
}