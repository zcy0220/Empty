# 网络层
* [构建本地服务器](../../Server/SERVER.md)
* 测试场景：Scenes->Test->NetworkManagerTest
* Server/proto：确定协议内容
* Server/NetMsgConfig：确定协议号与协议的对应配置
* Tools/Proto/ExportCSNetMsg：自动导出

## Proto定义
~~~C#
syntax="proto2";

package User;

// 登录请求
message LoginRequest
{
	required string Account = 1;
}

// 登录响应
message LoginResponse
{
    required int32 Result = 1;
    optional User User = 2;
}

// 用户数据
message User
{
    required Base Base = 1;
    repeated Item Items = 2;
}

// 用户基础数据
message Base
{
    required int32 UID = 1;
    required string Name = 2;
}

// 背包道具
message Item
{
    required int32 Id = 1;
    required int32 Num = 2;
}
~~~

## NetMsgConfig
~~~json
[
    {"MsgId":1001, "MsgName":"login", "Request":"User.LoginRequest", "Response":"User.LoginResponse"}
]
~~~

## C#使用测试
~~~C#
/// <summary>
/// 发送数据
/// </summary>
// NetworkManager.Instance.Send(NetMsg.LOGIN, new User.LoginRequest() { Account = "TestUser" });
/// <summary>
/// 监听服务器的消息回调
/// </summary>
// NetworkManager.Instance.AddNetMsgEventListener(NetMsg.LOGIN, OnLogin);
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
~~~


