/**
 * 网络测试
 */

using UnityEngine;
using UnityEngine.UI;
using Base.Debug;

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
        NetManager.Instance.AddEventListener(NetMsg.LOGIN, OnLogin);
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
        NetManager.Instance.Send(NetMsg.LOGIN, new User.LoginRequest() { Account = "TestUser" });
    }

    /// <summary>
    /// 销毁是删除监听
    /// </summary>
    private void OnDestroy()
    {
        this.RemoveEventListener(EventMsg.NET_CONNECT_SUCCESS, OnConnectSuccess);
        NetManager.Instance.RemoveEventListener(NetMsg.LOGIN, OnLogin);
    }

    /// <summary>
    /// 连接成功
    /// </summary>
    private void OnConnectSuccess(object obj)
    {
        Debugger.Log("连接成功");
        MessageText.text = "Connect Success!";
    }

    /// <summary>
    /// 登录成功监听
    /// </summary>
    /// <param name="arg"></param>
    public void OnLogin(object arg)
    {
        var data = (User.LoginResponse)arg;
        if (data.Result == 0)
        {
            Debugger.Log("User UID: " + data.User.Base.UID);
            Debugger.Log("User Name: " + data.User.Base.Name);
            for (var i = 0; i < data.User.Items.Count; i++)
            {
                var item = data.User.Items[i];
                Debugger.Log("User Items[{0}]: Id[{1}], Num[{2}]", i, item.Id, item.Num);
            }
        }
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        GameObject.DestroyImmediate(gameObject);
        NetManager.Instance.Disconnect();
#endif
    }
}
