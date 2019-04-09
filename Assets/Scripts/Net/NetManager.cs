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
/// 响应委托
/// </summary>
public delegate void OnResponse(object obj);

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
    //private Thread mRecvThread;
    /// <summary>
    /// 发送环形缓存队列
    /// </summary>
    private CircularQueue<Protocol> mSendBuffer = new CircularQueue<Protocol>(BUFFERSIZE);
    /// <summary>
    /// 接受缓存
    /// </summary>
    //private CircularQueue<int> mRecvBuffer = new CircularQueue<int>(BUFFERSIZE);
    /// <summary>
    /// 网络连接状态
    /// </summary>
    public ENetState NetState { get; private set; }
    /// <summary>
    /// 协议回来的响应列表
    /// </summary>
    //private Dictionary<int, List<OnResponse>> mResponseList = new Dictionary<int, List<OnResponse>>();

    //private byte[] mReadBuffer = new byte[BUFFERSIZE];

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
    /// 计算包大小，发送给服务器
    /// </summary>
    private void SendMsg(int msgId, byte[] buffer)
    {
        // 总长度 = 数据包长度所占字节 + 协议号长度 + 协议内容长度
        var totalSize = 4 + 4 + buffer.Length;
        // todo 优化
        // 对象池缓存下ByteBuffer
        var byteBuffer = new ByteBuffer(totalSize);
        // 写入数据长度
        byteBuffer.WriteInt(totalSize - 4);
        // 写入协议号
        byteBuffer.WriteInt(msgId);
        // 写入协议内容
        byteBuffer.WriteBytes(buffer);
        // 发送
        mStream.Write(byteBuffer.GetBytes(), 0, totalSize);
        mStream.Flush();
    }

    /// <summary>
    /// 接收服务器消息
    /// </summary>
    private void OnReceive(IAsyncResult ar)
    {
        //var respond = ProtobufUtil.NDeserialize<Example>(receiveData);
        //Debugger.Log(respond);
        //Debugger.Log("服务器接受:" + respond.ExampleInt);
        //var test = new Example() { ExampleInt = -1, ExampleFloat = -2.5f, ExampleString = "cba", ExampleArray = new List<Item>() { new Item() { ItemDouble = 1.2, ItemBool = true } } };

        //var buf = ProtobufUtil.NSerialize(test);
        //var respond = ProtobufUtil.NDeserialize<Example>(buf);
        //Debugger.Log(respond);
        //var count = mTcpClient.Client.EndReceive(ar);
        //var str = System.Text.Encoding.UTF8.GetString(mReadBuffer, 0, count);
        //Debugger.Log(str);
        //mTcpClient.Client.BeginReceive(mReadBuffer, 0, BUFFERSIZE, SocketFlags.None, OnReceive, null);
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
    }
}
