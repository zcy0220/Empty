/**
 * 配置定义
 */

public static class AppConfig
{
    /// <summary>
    /// 编辑环境下才可以选择是否用AssetBundle进行资源控制
    /// 正式环境一律用AssetBundle
    /// </summary>
    public static bool UseAssetBundle = true;
    /// <summary>
    /// 是否检测版本更新
    /// </summary>
    public static bool CheckVersionUpdate = true;
    /// <summary>
    /// 服务器资源下载地址
    /// </summary>
    public static readonly string ServerResourceURL = "http://127.0.0.1:8000/";
}