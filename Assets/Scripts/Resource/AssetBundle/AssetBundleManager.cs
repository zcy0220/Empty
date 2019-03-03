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
    /// <summary>
    /// AssetBundleManifest
    /// </summary>
    private AssetBundleManifest mAssetBundleManifest;
    /// <summary>
    /// 路径和AB包对应表
    /// </summary>
    private Dictionary<string, string> mPathBundleDict = new Dictionary<string, string>();
    /// <summary>
    /// path为key
    /// </summary>
    private Dictionary<string, AssetBundleUnit> mAssetBundleUnitDict = new Dictionary<string, AssetBundleUnit>();
    /// <summary>
    /// AssetBundleUnit的类对象池
    /// </summary>
    private ClassObjectPool<AssetBundleUnit> mAssetBundleUnitPool = ObjectManager.Instance.GetOrCreateClassPool<AssetBundleUnit>();
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
            LoadAssetBundleManifest();
        }
        return mAssetBundleManifest;
    }

    /// <summary>
    /// 加载AssetBundle配置文件
    /// </summary>
    private void LoadAssetBundleManifest()
    {
        var manifestBundle = AssetBundle.LoadFromFile(AssetBundleConfig.AssetBundlesPath + "/AssetBundles");
        mAssetBundleManifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        var configAssetBundle = AssetBundle.LoadFromFile(StringUtil.Concat(AssetBundleConfig.AssetBundlesPath, "/", AssetBundleConfig.PathBundleConfig.ToLower()));
        var configTextAsset = configAssetBundle.LoadAsset<TextAsset>(AssetBundleConfig.PathBundleConfig);
        var pathBundleConfigList = ProtobufUtil.NDeserialize<PathBundleInfoList>(configTextAsset.bytes);
        foreach(var info in pathBundleConfigList.List)
        {
            mPathBundleDict[info.Path] = info.AssetBundleName;
        }
    }

    /// <summary>
    /// 同步加载
    /// </summary>
    public T SyncLoad<T>(string path) where T : Object
    {
        var assetBundle = SyncLoadAllAssetBundle(path);
        return assetBundle.LoadAsset<T>(path);
    }

    /// <summary>
    /// 同步加载所有依赖Bundle
    /// </summary>
    /// <param name="path">Path.</param>
    public AssetBundle SyncLoadAllAssetBundle(string path)
    {
        var manifest = GetAssetBundleManifest();
        var assetBundleName = PathToBundle(path);
        var dependencies = manifest.GetAllDependencies(assetBundleName);
        foreach (var depend in dependencies)
        {
            SyncLoadAssetBundle(depend);
        }
        return SyncLoadAssetBundle(assetBundleName);
    }

    /// <summary>
    /// 路径对应的AssetBundle包名
    /// </summary>
    private string PathToBundle(string path)
    {
        if (mPathBundleDict.ContainsKey(path))
        {
            return mPathBundleDict[path];
        }
        else
        {
            Debugger.LogError("{0} not exist AssetBundleName!", path);
            return string.Empty;
        }
    }

    /// <summary>
    /// Gets the asset bundle unit.
    /// </summary>
    public AssetBundleUnit GetAssetBundleUnit(string path)
    {
        AssetBundleUnit unit;
        mAssetBundleUnitDict.TryGetValue(path, out unit);
        return unit;                
    }

    /// <summary>
    /// 根据AB包名同步加载AssetBundle
    /// </summary>
    private AssetBundle SyncLoadAssetBundle(string assetBundleName)
    {
        AssetBundleUnit unit = null;
        if (!mAssetBundleUnitDict.TryGetValue(assetBundleName, out unit))
        {
            var path = StringUtil.Concat(AssetBundleConfig.AssetBundlesPath, "/", assetBundleName);
            var assetBundle = AssetBundle.LoadFromFile(path);
            if (assetBundle == null)
            {
                Debugger.LogError("Load AssetBundle Error: " + assetBundleName);
                return null;
            }
            unit = mAssetBundleUnitPool.Spawn();
            unit.AssetBundle = assetBundle;
            unit.RefCount++;
            mAssetBundleUnitDict.Add(assetBundleName, unit);
        }
        else
        {
            unit.RefCount++;
        }
        return unit.AssetBundle;
    }

    /// <summary>
    /// 卸载AssetBundle
    /// </summary>
    private void UnloadAssetBundle(string abName)
    {
        if (mAssetBundleUnitDict.ContainsKey(abName))
        {
            var unit = mAssetBundleUnitDict[abName];
            unit.RefCount--;
            if (unit.RefCount <=0 && unit.AssetBundle != null)
            {
                unit.AssetBundle.Unload(true);
                unit.Reset();
                mAssetBundleUnitPool.Recycle(unit);
                mAssetBundleUnitDict.Remove(abName);
            }
        }
    }

    /// <summary>
    /// 卸载对应路径的AB包
    /// </summary>
    public void Unload(string path)
    {
        var assetBundleName = PathToBundle(path);
        var manifest = GetAssetBundleManifest();
        var dependencies = manifest.GetAllDependencies(assetBundleName);
        UnloadAssetBundle(assetBundleName);
        foreach (var depend in dependencies)
        {
            UnloadAssetBundle(depend);
        }
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

    /// <summary>
    /// 回收
    /// </summary>
    public void Reset()
    {
        RefCount = 0;
        AssetBundle = null;
    }
}