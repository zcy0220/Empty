/**
 * 路径处理工具
 */

using UnityEngine;
using Base.Utils;

public class PathUtil
{
    /// <summary>
    /// 获得本地资源文件路径
    /// 热更新资源会写入到PresistentData目录下
    /// 本地资源先查询PresistentData目录，没有则返回StreamingAssets目录下路径
    /// </summary>
    public static string GetLocalFilePath(string filePath)
    {
        return CheckPresistentDataFileExsits(filePath) ? GetPresistentDataFilePath(filePath) : GetStreamingAssetsFilePath(filePath);
    }
    
    /// <summary>
    /// 获得StreamingAssets下的资源文件路径
    /// </summary>
    public static string GetStreamingAssetsFilePath(string filePath)
    {
        return StringUtil.PathConcat(Application.streamingAssetsPath, filePath);
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
}
