/**
 * AssetBundle资源管理
 */

using Base.Common;
using UnityEngine;
using Base.Debug;
using Base.Pool;
using System.IO;
using System.Collections.Generic;
using Base.Utils;

public class AssetBundleManager : MonoSingleton<AssetBundleManager>
{
    private AssetBundleManifest mAssetBundleManifest;
    private Dictionary<string, string> mPathBundleDict = new Dictionary<string, string>();
    private Dictionary<string, AssetBundleUnit> mAssetBundleUnitDict = new Dictionary<string, AssetBundleUnit>();
    private ClassObjectPool<AssetBundleUnit> mAssetBundleUnitPool = ObjectManager.Instance.GetOrCreateClassPool<AssetBundleUnit>();

    /// <summary>
    /// 以path为key存储一份AssetBundleBase
    /// </summary>
    private Dictionary<string, AssetBundleBase> mAssetBundleBaseDict = new Dictionary<string, AssetBundleBase>();
    /// <summary>
    /// 存储加载的AssetBundleItem
    /// </summary>
    private Dictionary<string, AssetBundleItem> mAssetBundleItemDict = new Dictionary<string, AssetBundleItem>();
    /// <summary>
    /// AssetBundleItem对应的类对象池
    /// </summary>
    private ClassObjectPool<AssetBundleItem> mAssetBundleItemPool = ObjectManager.Instance.GetOrCreateClassPool<AssetBundleItem>();
    /// <summary>
    /// 异步加载资源请求队列
    /// </summary>
    private List<AssetLoadRequest> mAssetRequestQueue = new List<AssetLoadRequest>();
    /// <summary>
    /// 正在异步加载的队列
    /// </summary>
    private HashSet<AssetLoader> mAssetLoadingQueue = new HashSet<AssetLoader>();
    /// <summary>
    /// 资源加载请求AssetLoadRequest类对象池
    /// </summary>
    private ClassObjectPool<AssetLoadRequest> mAssetLoadRequestPool = ObjectManager.Instance.GetOrCreateClassPool<AssetLoadRequest>();
    /// <summary>
    /// AssetLoader类对象池
    /// </summary>
    private ClassObjectPool<AssetLoader> mAssetLoaderPool = ObjectManager.Instance.GetOrCreateClassPool<AssetLoader>();
    /// <summary>
    /// 同时在异步加载的资源上限
    /// </summary>
    private const int MAXLOADNUM = 5;

    /// <summary>
    /// 获得AssetBundleManifest
    /// </summary>
    private AssetBundleManifest GetAssetBundleManifest()
    {
        if (mAssetBundleManifest == null)
        {
            LoadAssetBundleConfig();
        }
        return mAssetBundleManifest;
    }

    /// <summary>
    /// 加载AssetBundle配置文件
    /// </summary>
    private void LoadAssetBundleConfig()
    {
        var manifestBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundles/AssetBundles");
        mAssetBundleManifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
    }

    /// <summary>
    /// 同步加载
    /// </summary>
    public T SyncLoad<T>(string path) where T : Object
    {
        var mainfest = GetAssetBundleManifest();
        
        var dependencies = manifest.GetAllDependencies("assets/gameassets/prefabs/exampleprefab1.prefab");
        return null;
    }

    public T SyncLoadAsset<T>(string assetBundleName) where T : Object
    {
        //AssetBundleUnit unit;
        //if (mAssetBundleUnitDict.TryGetValue(assetBundleName, out unit))
        //{
        //    unit.RefCount++;
        //}
        //else
        //{
            
        //}
        //if (mAssetBundleUnitDict)
        //var assetBundle = AssetBundle.LoadFromFile(StringUtil.Concat(AssetBundleConfig.AssetBundlesPath, "/", assetBundleName));
        //var manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
    }

    /// <summary>
    /// 获取ABBase资源
    /// </summary>
    public AssetBundleBase GetAssetBundleBase(string path)
    {
        if (mAssetBundleBaseDict.ContainsKey(path))
        {
            return mAssetBundleBaseDict[path];
        }
        return null;
    }

    /// <summary>
    /// 获取AssetBundle缓存
    /// </summary>
    public AssetBundleItem GetCacheAssetBundle(string abName)
    {
        if (mAssetBundleItemDict.ContainsKey(abName))
        {
            return mAssetBundleItemDict[abName];
        }
        return null;
    }

    /// <summary>
    /// 根据AB包名同步加载AssetBundle
    /// </summary>
    private AssetBundle SyncLoadAssetBundle(string abName)
    {
        AssetBundleUnit unit = null;
        if (!mAssetBundleUnitDict.TryGetValue(abName, out unit))
        {
            AssetBundle assetBundle = null;
            var path = StringUtil.Concat(AssetBundleConfig.AssetBundlesPath, "/", abName);
            assetBundle = AssetBundle.LoadFromFile(path);
            if (assetBundle == null)
            {
                Debugger.LogError("Load AssetBundle Error: " + abName);
                return null;
            }
            unit = mAssetBundleUnitPool.Spawn();
            unit.AssetBundle = assetBundle;
            unit.RefCount++;
            mAssetBundleUnitDict.Add(abName, unit);
        }
        else
        {
            unit.RefCount++;
        }
        return unit.AssetBundle;
    }

    /// <summary>
    /// 加载AssetBundleBase所依赖的所有AB包
    /// </summary>
    public AssetBundle SyncLoadAssetBundle(AssetBundleBase abBase)
    {
        if (abBase == null) return null;
        var ab = SyncLoadAssetBundle(abBase.ABName);
        if (abBase.ABDependList != null)
        {
            for (var i = 0; i < abBase.ABDependList.Count; i++)
            {
                SyncLoadAssetBundle(abBase.ABDependList[i]);
            }
        }
        return ab;
    }

    /// <summary>
    /// 卸载AssetBundle
    /// </summary>
    private void UnloadAssetBundle(string abName)
    {
        AssetBundleItem item = null;
        if (mAssetBundleItemDict.TryGetValue(abName, out item) && item != null)
        {
            item.RefCount--;
            if (item.RefCount <= 0 && item.AssetBundle != null)
            {
                item.AssetBundle.Unload(true);
                item.Reset();
                mAssetBundleItemPool.Recycle(item);
                mAssetBundleItemDict.Remove(abName);
            }
        }
    }

    /// <summary>
    /// 根据path找到对应的ABBase,卸载关联的ab包
    /// </summary>
    public void UnloadAssetBundleByPath(string path)
    {
        var abBase = GetAssetBundleBase(path);
        if (abBase == null) return;
        if (abBase.ABDependList != null)
        {
            for (var i = 0; i < abBase.ABDependList.Count; i++)
            {
                UnloadAssetBundle(abBase.ABDependList[i]);
            }
        }
        UnloadAssetBundle(abBase.ABName);
    }

    /// <summary>
    /// 添加异步加载请求
    /// </summary>
    public void AddAssetLoadRequest(string path, System.Action<Object> callback, int priority = 0)
    {
        var request = mAssetLoadRequestPool.Spawn();
        request.Init();
        request.Path = path;
        request.Callback = callback;
        request.Priority = priority;
        mAssetRequestQueue.Add(request);
    }

    /// <summary>
    /// 检测资源是否已经在加载队列中了
    /// </summary>
    private AssetLoader AssetInLoading(string path)
    {
        foreach(var loader in mAssetLoadingQueue)
        {
            if (string.Compare(path, loader.Path) == 0) return loader;
        }
        return null;
    }

    /// <summary>
    /// 检测请求队列中的资源，并加入到要加载的队列中
    /// </summary>
    private void CheckAssetRequestQueue()
    {
        if (mAssetRequestQueue.Count == 0) return;
        // 正在加载的资源达到上限
        if (mAssetLoadingQueue.Count >= MAXLOADNUM) return;
        // 按优先级排序
        mAssetRequestQueue.Sort();
        while (mAssetLoadingQueue.Count < MAXLOADNUM)
        {
            if (mAssetRequestQueue.Count == 0) break;

            // 从请求队列中拿一个加入到加载队列中
            var request = mAssetRequestQueue[0];
            mAssetRequestQueue.RemoveAt(0);
            var loader = AssetInLoading(request.Path);
            if (loader != null)
            {
                // 如果请求加载的资源在请求队列中，就增加回调
                loader.AddCallback(request.Callback);
            }
            else
            {
                loader = mAssetLoaderPool.Spawn();
                loader.Path = request.Path;
                loader.AddCallback(request.Callback);
                mAssetLoadingQueue.Add(loader);
            }
            request.Dispose();
            mAssetLoadRequestPool.Recycle(request);
        }
    }

    /// <summary>
    /// 处理正在加载队列
    /// </summary>
    public void DealAssetLoadingQueue()
    {
        foreach(var loader in mAssetLoadingQueue)
        {
            if (loader.CanLoadAssetAsync())
            {
                StartCoroutine(loader.LoadAssetAsync());
            }
        }
    }

    /// <summary>
    ///  资源加载器完成
    /// </summary>
    public void AssetLoaderFinished(AssetLoader loader)
    {
        if (mAssetLoadingQueue.Contains(loader))
        {
            mAssetLoadingQueue.Remove(loader);
        }
        loader.Dispose();
        mAssetLoaderPool.Recycle(loader);
    }

    /// <summary>
    /// 检测
    /// </summary>
    private void Update()
    {
        CheckAssetRequestQueue();
        DealAssetLoadingQueue();
    }
}


public class AssetBundleUnit
{
    /// <summary>
    /// 对应的AssetBundle引用
    /// </summary>
    public AssetBundle AssetBundle;
    /// <summary>
    /// 引用计数
    /// </summary>
    public int RefCount;
}