/**
 * 网络管理
 */

using Proto;
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
    ///// <summary>
    ///// 发送线程
    ///// </summary>
    //private Thread mSendThread;
    ///// <summary>
    ///// 接受线程
    ///// </summary>
    //private Thread mRecvThread;
    /// <summary>
    /// 发送缓存
    /// </summary>
    private CircularQueue<int> mSendBuffer = new CircularQueue<int>(BUFFERSIZE);
    /// <summary>
    /// 接受缓存
    /// </summary>
    private CircularQueue<int> mRecvBuffer = new CircularQueue<int>(BUFFERSIZE);
    /// <summary>
    /// 网络连接状态
    /// </summary>
    public ENetState NetState { get; private set; }
    /// <summary>
    /// 协议回来的响应列表
    /// </summary>
    private Dictionary<int, List<OnResponse>> mResponseList = new Dictionary<int, List<OnResponse>>();

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
            Debugger.LogError("Connect {0} failed! error: {1}", mHost, e.Message);
            this.DispatchEvent(EventMsg.NET_CONNECT_FAILED);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void OnConnectSuccess()
    {
        // 网络状态设为连接中
        NetState = ENetState.CONNECTING;
        //this.DispatchEvent(EventMsg.NET_CONNECT_SUCCESS);
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
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public void Disconnect()
    {
        if (mTcpClient != null)
        {
            mTcpClient.Close();
        }
    }

    /// <summary>
    /// 发送协议消息
    /// </summary>
    public void Send<T>(int msgId, T request)
    {
        if (mTcpClient == null) return;
        var buffer = ProtobufUtil.NSerialize(request);
        //mTcpClient.Client.Send(buffer);
        mStream.Write(buffer, 0, buffer.Length);
        mStream.Flush();
    }
    
    /// <summary>
    /// 连接的回调并不在主线程中，要在Update中检测网络连接状态
    /// </summary>
    private void Update()
    {
    }

    #region TEST
    /// <summary>
    /// 连接成功后测试发送协议
    /// </summary>
    public void OnConnectSuccess(object[] param)
    {
        var request = new Example();
        request.ExampleInt = 1;
        request.ExampleFloat = 2.5f;
        request.ExampleString = "abc";
        request.ExampleArray = new List<Item>() { new Item() { ItemBool = true, ItemDouble = 3.5 }, new Item() { ItemBool = false, ItemDouble = 4.5 } };
        Send(0, request);
    }
    #endregion
}
