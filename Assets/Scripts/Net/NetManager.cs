/**
 * 网络管理
 */

using System;
using Base.Common;
using Base.Debug;
using System.Net.Sockets;

public class NetManager : Singleton<NetManager>
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
    void OnConnect(IAsyncResult ar)
    {
        try
        {
            mTcpClient.GetStream();
            Debugger.Log("Connect Success");
        }
        catch (Exception e)
        {
            Debugger.LogError(e.Message);
        }
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
        //mTcpClient.Client.Send()
    }

    /// <summary>
    /// 测试用例
    /// </summary>
    public void Test()
    {
        NetManager.Instance.Connect(AppConfig.ServerHost, AppConfig.ServerPort);
    }
}
