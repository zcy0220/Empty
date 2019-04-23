/**
 * 网络连接状态
 */

namespace Base.Network
{
    public enum ESocketState
    {
        /// <summary>
        /// 关闭状态
        /// </summary>
        CLOSED,
        /// <summary>
        /// 连接状态
        /// </summary>
        CONNECTED,
        /// <summary>
        /// 连接中
        /// </summary>
        CONNECTING,
    }
}
