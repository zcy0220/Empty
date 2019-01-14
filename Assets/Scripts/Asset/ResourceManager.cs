/**
 * 资源加载管理类
 */

using Base.Common;
using Base.Pool;
using UnityEngine;
using Base.Utils;
using Base.Debug;
using System.Collections.Generic;

public class ResourceManager : Singleton<ResourceManager>
{
    /// <summary>
    /// 同步资源加载器
    /// </summary>
    private AssetLoader mSyncLoader;
    /// <summary>
    /// 资源缓存
    /// </summary>
    private Dictionary<string, object> mAssetsCache = new Dictionary<string, object>();

    /// <summary>
    /// 创建对应平台的加载器
    /// </summary>
    public ResourceManager()
    {
        if (AppConfig.UseAssetBundle)
            mSyncLoader = PoolManager.Instance.Fetch<MobileAssetLoader>();
        else
            mSyncLoader = PoolManager.Instance.Fetch<EditorAssetLoader>();
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Release()
    {
        mAssetsCache.Clear();
    }

    /// <summary>
    /// 同步加载
    /// </summary>
    public T SyncLoad<T>(string assetPath) where T : Object
    {
        if (mSyncLoader == null) return null;
        return mSyncLoader.SyncLoad<T>(assetPath);
    }

    //======================AssetDatabase.LoadAssetAtPath 完整路径名(包括扩展名)======================
    //=================================需要拓展用到的类型的加载方法===================================
    public TextAsset LoadSheet(string path)
    {
        var assetPath = StringUtil.Concat(AssetPath.Resources, path, ".bytes");
        var asset = SyncLoad<TextAsset>(assetPath);
        Debugger.Log(asset == null, StringUtil.Concat(assetPath , " not exist!"));
        return asset;
    }
}
