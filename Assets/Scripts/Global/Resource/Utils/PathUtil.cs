/**
 * 路径处理工具
 */

using UnityEngine;
using Base.Utils;

public class PathUtil
{
    /// <summary>
    /// 资源完整路径的前缀
    /// </summary>
    public const string ASSETPATHPREFIX = "Assets/Package";

    /// <summary>
    /// 获得资源的完整路径
    /// </summary>
    public static string GetFullAssetPath(string assetPath)
    {
        return StringUtil.PathConcat(ASSETPATHPREFIX, assetPath);
    }

    /// <summary>
    /// 获得本地资源文件路径
    /// 热更新资源会写入到PresistentData目录下
    /// 本地资源先查询PresistentData目录，没有则返回StreamingAssets目录下路径
    /// </summary>
    public static string GetLocalFilePath(string filePath, bool www = false)
    {
        return CheckPresistentDataFileExsits(filePath) ? GetPresistentDataFilePath(filePath) : GetStreamingAssetsFilePath(filePath, www);
    }

    /// <summary>
    /// 获得StreamingAssets下的资源文件路径
    /// </summary>
    public static string GetStreamingAssetsFilePath(string filePath, bool www = false)
    {
        var streamingAssetsPath = Application.streamingAssetsPath;
#if UNITY_EDITOR
        if (www)
        {
            // Mac环境编辑模式下，用www加载StreamingAssets资源要加前缀
            streamingAssetsPath = StringUtil.Concat("file://", Application.streamingAssetsPath);
        }
#elif UNITY_IOS
        if (www)
        {
            // IOS用www加载StreamingAssets资源要加前缀
            streamingAssetsPath = StringUtil.Concat("file://", Application.streamingAssetsPath);
        }
#endif
        return StringUtil.PathConcat(streamingAssetsPath, filePath);
    }

    /// <summary>
    /// 获得PresistentData下的资源文件路径
    /// </summary>
    public static string GetPresistentDataFilePath(string filePath)
    {
        return StringUtil.PathConcat(Application.persistentDataPath, filePath); 
    }

    /// <summary>
    /// 检测PresistentData下的资源文件路径
    /// </summary>
    public static bool CheckPresistentDataFileExsits(string filePath)
    {
        var path = GetPresistentDataFilePath(filePath);
        return FileUtil.Exists(path);
    }

    /// <summary>
    /// 获取下载资源路径
    /// </summary>
    public static string GetServerFileURL(string filePath)
    {
        return StringUtil.Concat(AppConfig.ServerResourceURL, filePath); 
    }

    /// <summary>
    /// 获得本地AssetBundle路径
    /// </summary>
    public static string GetLocalAssetBundleFilePath(string assetBundleName)
    {
        var assetBundlesPath = StringUtil.PathConcat(AssetBundleConfig.AssetBundlesFolder, assetBundleName);
        return GetLocalFilePath(assetBundlesPath);
    }
}
