/**
 * AB资源控制
 */

using System;
using Base.Debug;

public class AssetBundleResourceLoader : BaseResourceLoader
{
    /// <summary>
    /// 同步加载资源
    /// </summary>
    public override T SyncLoad<T>(string path)
    {
        return AssetBundleManager.Instance.SyncLoad<T>(path);
    }

    /// <summary>
    /// AssetBundle的异步加载，添加异步加载请求
    /// </summary>
    public override void AsyncLoad(string path, Action<UnityEngine.Object> callback)
    {
        AssetBundleManager.Instance.AddAssetLoadRequest(path, callback);
    }

    /// <summary>
    /// 卸载对应的AB资源
    /// </summary>
    public override void Unload(string path)
    {
        AssetBundleManager.Instance.Unload(path);
    }
}
