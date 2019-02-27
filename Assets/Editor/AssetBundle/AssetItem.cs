/**
 * 资源节点信息
 */

using System.Collections.Generic;

namespace Assets.Editor.AssetBundle
{
    public class AssetItem
    {
        /// <summary>
        /// 该资源的所有依赖项
        /// </summary>
        public HashSet<string> Depends = new HashSet<string>();
        /// <summary>
        /// 依赖该资源的所有资源项
        /// </summary>
        public HashSet<string> BeDepends = new HashSet<string>();
    }
}
