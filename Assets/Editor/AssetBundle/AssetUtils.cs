/**
 * 资源工具类
 */

using System.Collections.Generic;
using System.IO;

namespace Assets.Editor.AssetBundle
{
    public class AssetUtils
    {
        /// <summary>
        /// 需要过滤的资源类型后缀
        /// </summary>
        public static readonly HashSet<string> FilterAssetTyteExtension = new HashSet<string>()
        {
            ".meta",
            ".cs",
            ".zip",
            ".DS_Store",
            ""
        };

        /// <summary>
        /// 是否为有效的资源路径
        /// </summary>
        public static bool ValidAsset(string path)
        {
            return !FilterAssetTyteExtension.Contains(Path.GetExtension(path));
        }
    }
}
