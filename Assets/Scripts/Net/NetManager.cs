/**
 * 网络管理
 */

using Proto;
using System;
using Base.Common;
using Base.Debug;
using Base.Utils;
using System.Net.Sockets;
using System.Collections.Generic;

public class NetManager : Singleton<NetManager>, IEventDispatcher, IEventReceiver
{
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
    private NetworkStream mStream;

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
            return false;
        }
    }

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
    /// 连接上服务器
    /// </summary>
    private byte[] receiveData = new byte[34];
    private void OnConnect(IAsyncResult ar)
    {
        try
        {
            Debugger.Log("Connect Success");
            mStream = mTcpClient.GetStream();
            //var socket = mTcpClient.Client;
            this.DispatchEvent(EventMsg.NET_CONNECT_SUCCESS);
            //socket.BeginReceive(receiveData, 0, receiveData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
        }
        catch (Exception e)
        {
            Debugger.LogError(e.Message);
        }
    }

    /// <summary>
    /// 接收服务器消息
    /// </summary>
    private void OnReceive(IAsyncResult ar)
    {
        var respond = ProtobufUtil.NDeserialize<Example>(receiveData);
        Debugger.Log(respond);
        Debugger.Log("服务器接受:" + respond.ExampleInt);
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

    #region TEST
    /// <summary>
    /// 测试用例
    /// </summary>
    public void Test()
    {
        Connect(AppConfig.ServerHost, AppConfig.ServerPort);
        this.AddEventListener(EventMsg.NET_CONNECT_SUCCESS, OnConnectSuccess);
    }

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
