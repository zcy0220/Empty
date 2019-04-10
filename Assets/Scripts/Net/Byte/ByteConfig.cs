/**
 * 字节缓存配置
 * Size配置根据实际项目
 * 一般情况发送Size可以小点，接收Size大一点
 * 比如登录协议，客户端只用发送用户名等信息，但服务器会下发整个背包数据等信息
 */

public class ByteConfig
{
    /// <summary>
    /// 发送buffer最大Size
    /// </summary>
    public const int MAX_SEND_SIZE = 1024;
    /// <summary>
    /// 接收buffer最大Size
    /// </summary>
    public const int MAX_RECV_SIZE = 1024;
}
