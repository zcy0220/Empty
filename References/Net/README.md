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

## 接收处理网络包代码
~~~C#
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
~~~


