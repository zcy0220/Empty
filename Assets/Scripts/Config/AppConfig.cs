/**
 * 配置定义
 */

public static class AppConfig
{
    /// <summary>
    /// 编辑环境下才可以选择是否用AssetBundle进行资源控制
    /// 正式环境一律用AssetBundle
    /// </summary>
    public static bool UseAssetBundle = false;
    /// <summary>
    /// 是否检测版本更新
    /// </summary>
    public static bool CheckVersionUpdate = true;
}