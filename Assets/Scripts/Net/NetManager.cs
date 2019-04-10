/**
 * 网络管理
 */

using System;
using Base.Common;
using Base.Debug;
using Base.Utils;
using Base.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

/// <summary>
/// 网络连接状态
/// </summary>
public enum ENetState
{
    NONE,
    SUCCESS,
    CONNECTING,
}

public class NetManager : MonoSingleton<NetManager>, IEventDispatcher
{
    /// <summary>
    /// 缓存池大小
    /// </summary>
    const int BUFFERSIZE = 1024;
    /// <summary>
    /// 主机
    /// </summary>
    private string mHost;
    /// <summary>
    /// 端口
    /// </summary>
    private int mPort;
    /// <summary>
    /// socket
    /// </summary>
    private TcpClient mTcpClient;
    /// <summary>
    /// NetworkStream
    /// </summary>
    private NetworkStream mStream;
    /// <summary>
    /// 控制发送和接收线程状态的标志位
    /// </summary>
    private bool mThreadStateFlag = false;
    ///// <summary>
    ///// 发送线程
    ///// </summary>
    private Thread mSendThread;
    ///// <summary>
    ///// 接受线程
    ///// </summary>
    private Thread mRecvThread;
    /// <summary>
    /// 发送环形缓存队列
    /// </summary>
    private CircularQueue<Protocol> mSendBuffer = new CircularQueue<Protocol>(BUFFERSIZE);
    /// <summary>
    /// 接受缓存
    /// </summary>
    private CircularQueue<Protocol> mRecvBuffer = new CircularQueue<Protocol>(BUFFERSIZE);
    /// <summary>
    /// 网络连接状态
    /// </summary>
    public ENetState NetState { get; private set; }

    /// <summary>
    /// 连接
    /// </summary>
    public bool Connect(string host, int port)
    {
        mHost = host;
        mPort = port;
        return Connect();
    }

    /// <summary>
    /// 连接
    /// </summary>
    public bool Connect()
    {
        try
        {
            Disconnect();
            mTcpClient = new TcpClient();
            mTcpClient.BeginConnect(mHost, mPort, new AsyncCallback(OnConnect), null);
            return true;
        }
        catch
        {
            Debugger.LogError("Connect error host: {0}, port: {1}", mHost, mPort);
            return false;
        }
    }

    /// <summary>
    /// 连接上服务器
    /// </summary>
    private void OnConnect(IAsyncResult ar)
    {
        try
        {
            mStream = mTcpClient.GetStream();
            NetState = ENetState.SUCCESS;
        }
        catch(Exception e)
        {
            Debugger.LogError("Connect {0}:{1} Failed! error: {2}", mHost, mPort, e.Message);
            this.DispatchEvent(EventMsg.NET_CONNECT_FAILED);
        }
    }
    
    /// <summary>
    /// 连接成功后 切换网络状态 同时开启发送和接收线程
    /// </summary>
    public void OnConnectSuccess()
    {
        // 网络状态设为连接中
        NetState = ENetState.CONNECTING;
        StartNetThread();
        this.DispatchEvent(EventMsg.NET_CONNECT_SUCCESS);
    }
    
    /// <summary>
    /// 开启发送和接收线程
    /// </summary>
    private void StartNetThread()
    {
        mThreadStateFlag = true;
        mSendThread = new Thread(SendFunc);
        mSendThread.Start();
        mRecvThread = new Thread(RecvFunc);
        mRecvThread.Start();
    }
    
    /// <summary>
    /// 发送线程执行的发送方法
    /// </summary>
    private void SendFunc()
    {
        while(mThreadStateFlag)
        {
            if (!mSendBuffer.IsEmpty)
            {
                lock (mSendBuffer)
                {
                    var protocol = mSendBuffer.Dequeue();
                    SendMsg(protocol.MsgId, protocol.Buffer);
                }
            }
        }
    }

    /// <summary>
    /// 接收线程执行的接收方法
    /// </summary>
    private void RecvFunc()
    {
        var byteBuffer = ByteBufferPool.Instance.Spawn(ByteConfig.MAX_RECV_SIZE);
        var buf = byteBuffer.GetBytes();
        while(mThreadStateFlag)
        {
            try
            {
                var socket = mTcpClient.Client;
                var length = mStream.Read(buf, 0, byteBuffer.Capacity);
                if (length > 8)
                {
                    var size = byteBuffer.ReadInt(0);
                    var msgId = byteBuffer.ReadInt(4);
                    // todo 优化接收buffer缓存
                    var recvBuffer = new byte[size - 4];
                    Buffer.BlockCopy(buf, 8, recvBuffer, 0, size - 4);
                    var protocol = new Protocol(msgId, recvBuffer);
                    lock (mRecvBuffer)
                    {
                        mRecvBuffer.Enqueue(protocol);
                    }
                }
            }
            catch
            {
                Debugger.LogError("Socket Close!");
            }
        }
        ByteBufferPool.Instance.Recycle(byteBuffer);
    }

    /// <summary>
    /// 计算包大小，发送给服务器
    /// </summary>
    private void SendMsg(int msgId, byte[] buffer)
    {
        // 总长度 = 数据包长度所占字节 + 协议号长度 + 协议内容长度
        var totalSize = 4 + 4 + buffer.Length;
        // todo 优化
        // 对象池缓存下ByteBuffer
        var byteBuffer = ByteBufferPool.Instance.Spawn(ByteConfig.MAX_SEND_SIZE);
        // 写入数据长度
        byteBuffer.WriteInt(totalSize - 4);
        // 写入协议号
        byteBuffer.WriteInt(msgId);
        // 写入协议内容
        byteBuffer.WriteBytes(buffer);
        // 发送
        mStream.Write(byteBuffer.GetBytes(), 0, totalSize);
        mStream.Flush();
        // 回收ByteBuffer
        ByteBufferPool.Instance.Recycle(byteBuffer);
    }
    
    /// <summary>
    /// 字节流发送协议消息 Lua使用
    /// </summary>
    public void Send(int msgId, byte[] buffer)
    {
        var protocol = new Protocol(msgId, buffer);
        lock (mSendBuffer)
        {
            mSendBuffer.Enqueue(protocol);
        }
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
    /// 连接的回调并不在主线程中，要在Update中检测网络连接状态
    /// </summary>
    private void Update()
    {
        if (NetState == ENetState.SUCCESS)
        {
            OnConnectSuccess();
        }
        HandleRecvMsg();
    }

    /// <summary>
    /// 处理接收的消息
    /// </summary>
    private void HandleRecvMsg()
    {
        if (mRecvBuffer.IsEmpty) return;
        lock (mRecvBuffer)
        {
            var protocol = mRecvBuffer.Dequeue();
            var msgId = protocol.MsgId;
            var type = NetMsg.GetTypeByMsgId(msgId);
            var response = ProtobufUtil.NDeserialize(type, protocol.Buffer);
            Debugger.Log(msgId);
            Debugger.Log(response);
            HandlerEventListener(msgId, response);
        }
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public void Disconnect()
    {
        if (!mThreadStateFlag) return;
        mTcpClient.Close();
        mStream.Close();
        mThreadStateFlag = false;
        NetState = ENetState.NONE;
        mSendThread.Join(1000);
        mRecvThread.Join(1000);
    }

    #region NetEvent
    /// <summary>
    /// 回调委托
    /// </summary>
    public delegate void OnResponse(object obj);
    /// <summary>
    /// 网络消息回调处理
    /// 因为这里是监听协议号处理回调的，所以没用EventManager，有空统一归到EventManager管理
    /// </summary>
    private Dictionary<int, List<OnResponse>> mResponseListDict = new Dictionary<int, List<OnResponse>>();

    /// <summary>
    /// 添加协议监听
    /// </summary>
    public void AddEventListener(int msgId, OnResponse callback)
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
    public void RemoveEventListener(int msgId, OnResponse callback)
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
