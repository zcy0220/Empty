/**
 * 协议配置文件
 */

using System.Collections.Generic;

namespace Editor.Proto
{
    public class NetMsgConfig<T>
    {
        public List<T> list;
    }

    [System.Serializable]
    public class NetMsg
    {
        /// <summary>
        /// 协议Id
        /// </summary>
        public int MsgId;
        /// <summary>
        /// 协议名(最终是配置名 + "_Msg"后缀再大写)
        /// </summary>
        public string MsgName;
        /// <summary>
        /// 请求
        /// </summary>
        public string Request;
        /// <summary>
        /// 响应
        /// </summary>
        public string Response;
    }
}
