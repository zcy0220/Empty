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
        //var abBase = AssetBundleManager.Instance.GetAssetBundleBase(path);
        //if (abBase != null)
        //{
        //    var ab = AssetBundleManager.Instance.SyncLoadAssetBundle(abBase);
        //    if (ab != null)
        //    {
        //        return ab.LoadAsset<T>(abBase.AssetName);
        //    }
        //}
        //else
        //{
        //    Debugger.LogError("No AssetBundleBase is found for the {0}", path);
        //}
        //return null;
        return AssetBundleManager.Instance.SyncLoad<T>(path);
    }

    /// <summary>
    /// AssetBundle的异步加载，添加异步加载请求
    /// </summary>
    public override void AsyncLoad(string path, Action<UnityEngine.Object> callback)
    {
        //AssetBundleManager.Instance.AddAssetLoadRequest(path, callback);
    }
}
