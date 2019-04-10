/**
 * 协议
 */

public class Protocol
{
    /// <summary>
    /// 协议号
    /// </summary>
    public int MsgId { get; private set; }
    /// <summary>
    /// 协议内容字节buffer
    /// </summary>
    public byte[] Buffer { get; private set; }

    /// <summary>
    /// 构造
    /// </summary>
    public Protocol(int msgId, byte[] buffer)
    {
        MsgId = msgId;
        Buffer = buffer;
    }
}
