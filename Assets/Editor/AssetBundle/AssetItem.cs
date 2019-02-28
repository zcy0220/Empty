/**
 * 资源节点信息
 */

using System.Collections.Generic;

namespace Assets.Editor.AssetBundle
{
    public class AssetItem
    {
        /// <summary>
        /// AB包名
        /// </summary>
        public string AssetBundleName = string.Empty;
        /// <summary>
        /// 该资源的所有依赖项
        /// </summary>
        public List<string> Depends = new List<string>();
        /// <summary>
        /// 依赖该资源的所有资源项
        /// </summary>
        public List<string> BeDepends = new List<string>();
    }
}
