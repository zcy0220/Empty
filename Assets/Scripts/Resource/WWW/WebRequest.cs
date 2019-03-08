/**
 * 文件资源请求
 */

namespace Resource
{
    public enum EWebRequestType
    {
        NONE = 0,
        NORMAL = 1,
        ASSETBUNDLE = 2
    }

    public abstract class WebRequest : AsyncLoader
    {
        /// <summary>
        /// 请求类型
        /// </summary>
        public EWebRequestType WebRequestType { get; protected set; }
        /// <summary>
        /// 资源名
        /// </summary>
        public string AssetName { get; protected set; }

        /// <summary>
        /// 开始执行
        /// </summary>
        public virtual void Start() { }
    }
}
