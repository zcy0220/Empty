/**
 * 路径处理工具
 */

using Base.Utils;

namespace Resource
{
    public class PathUtil
    {
        public static string GetStreamingAssetFilePath(string assetPath)
        {
#if UNITY_EDITOR
            string outputPath = StringUtil.PathConcat("file://", AssetBundleConfig.AssetBundlesPath);
#endif
            return StringUtil.PathConcat(outputPath, assetPath);
        }
    }
}
