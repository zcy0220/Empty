/**
 * 资源加载管理类
 */

using Base.Common;
using Base.Pool;
using System.Collections.Generic;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    /// <summary>
    /// 同步资源加载器
    /// </summary>
    private AssetLoader mSyncLoader;
    /// <summary>
    /// 资源缓存
    /// </summary>
    private Dictionary<string, object> mAssetsCache = new Dictionary<string, object>();

    public ResourceManager()
    {
        if (AppConfig.UseAssetBundle)
            mSyncLoader = PoolManager.Instance.Fetch<MobileAssetLoader>();
        else
            mSyncLoader = PoolManager.Instance.Fetch<EditorAssetLoader>();
    }
}
