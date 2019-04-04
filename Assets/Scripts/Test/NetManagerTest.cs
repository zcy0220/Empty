/**
 * 网络测试
 */

using UnityEngine;
using UnityEngine.UI;

public class NetManagerTest : MonoBehaviour, IEventReceiver
{
    public Text MessageText;

    /// <summary>
    /// 创建的时候添加监听
    /// </summary>
    private void Start()
    {
        NetManager.Instance.Connect(AppConfig.ServerHost, AppConfig.ServerPort);
        this.AddEventListener(EventMsg.NET_CONNECT_SUCCESS, OnConnectSuccess);
        this.AddEventListener(EventMsg.NET_CONNECT_FAILED, OnConnectFailed);
    }

    /// <summary>
    /// 销毁是删除监听
    /// </summary>
    private void OnDestroy()
    {
        this.RemoveEventListener(EventMsg.NET_CONNECT_SUCCESS, OnConnectSuccess);
        this.RemoveEventListener(EventMsg.NET_CONNECT_SUCCESS, OnConnectFailed);
    }

    /// <summary>
    /// 连接成功
    /// </summary>
    private void OnConnectSuccess(object obj)
    {
        Debug.Log("连接成功");
        MessageText.text = "Connect Success!";
    }
    
    /// <summary>
    /// 连接失败
    /// </summary>
    private void OnConnectFailed(object obj)
    {
        MessageText.text = "Connect Failed!";
    }

    private void Update()
    {
        if (NetManager.Instance.NetState == ENetState.SUCCESS)
        {
            //NetManager.Instance.OnConnectSuccess();
            MessageText.text = "Connect Success!";
        }
    }
}
