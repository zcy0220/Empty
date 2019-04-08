/**
 * 协议基类
 */

public abstract class ProtocolBase
{
    /// <summary>
    /// 解码
    /// </summary>
    /// <param name="buffer">字节buffer</param>
    /// <param name="index">开始位置</param>
    /// <param name="length">长度</param>
    /// <returns></returns>
    public abstract ProtocolBase Decode(byte[] buffer, int index, int length);

    /// <summary>
    /// 编码
    /// </summary>
    /// <returns></returns>
    public abstract byte[] Encode();
}
