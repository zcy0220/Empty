/**
 * 移动设备下的加载方式
 */

public class MobileAssetLoader : AssetLoader
{
    /// <summary>
    /// 移动平台下的同步加载
    /// </summary>
    public override T SyncLoad<T>(string path)
    {
        return null;
    }
}
