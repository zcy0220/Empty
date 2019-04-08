/**
 * 网络测试
 */

using UnityEngine;
using UnityEngine.UI;

public class NetManagerTest : MonoBehaviour, IEventReceiver
{
    public InputField Host;
    public InputField Port;
    public InputField Send;
    public Text MessageText;
    public Button ConnectBtn;
    public Button SendBtn;

    /// <summary>
    /// 创建的时候添加监听
    /// </summary>
    private void Start()
    {
        Host.text = AppConfig.ServerHost;
        Port.text = AppConfig.ServerPort.ToString();
        ConnectBtn.onClick.AddListener(OnConnectBtn);
        SendBtn.onClick.AddListener(OnSendBtn);
        this.AddEventListener(EventMsg.NET_CONNECT_SUCCESS, OnConnectSuccess);
    }
    
    /// <summary>
    /// 连接服务器
    /// </summary>
    private void OnConnectBtn()
    {
        NetManager.Instance.Connect(Host.text, int.Parse(Port.text));
    }
    
    /// <summary>
    /// 发送数据
    /// </summary>
    private void OnSendBtn()
    {
        NetManager.Instance.Send(Send.text);
    }

    /// <summary>
    /// 销毁是删除监听
    /// </summary>
    private void OnDestroy()
    {
        this.RemoveEventListener(EventMsg.NET_CONNECT_SUCCESS, OnConnectSuccess);
    }

    /// <summary>
    /// 连接成功
    /// </summary>
    private void OnConnectSuccess(object obj)
    {
        Debug.Log("连接成功");
        MessageText.text = "Connect Success!";
    }
}
