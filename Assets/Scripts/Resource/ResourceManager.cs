/**
 * 资源加载管理类
 */

using Base.Common;
using UnityEngine;
using Base.Debug;
using System.Collections.Generic;
using Base.Collections;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    /// <summary>
    /// 对应平台模式下的资源加载器
    /// </summary>
    private BaseResourceLoader mResourcesLoader;
    /// <summary>
    /// 缓存使用的资源列表
    /// </summary>
    private Dictionary<string, ResourceUnit> mResourceUnitDict = new Dictionary<string, ResourceUnit>();
    /// <summary>
    /// 缓存引用计数为0的资源, 达到缓存上限时情掉最早没用的资源(尾部资源)
    /// </summary>
    private DoubleLinkedList<ResourceUnit> mNoRefResourceUnitDList = new DoubleLinkedList<ResourceUnit>();

    /// <summary>
    /// 初始化
    /// </summary>
    private ResourceManager()
	{
        if (AppConfig.UseAssetBundle)
        {
            mResourcesLoader = new AssetBundleResourceLoader();
        }
        else
        {
            mResourcesLoader = new EditorResourceLoader();
        }
	}

	/// <summary>
	/// 同步加载
	/// </summary>
	public T SyncLoad<T>(string path) where T : Object
    {
        var cache = GetResourceUnitCache(path);
        if (cache != null)
        {
            return cache.Asset as T;
        }
        var obj = mResourcesLoader.SyncLoad<T>(path);
        CacheResourceUnit(path, obj);
        return obj;
    }

    /// <summary>
    /// 异步加载
    /// </summary>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    public void AsyncLoad<T>(string path, System.Action<Object> callback) where T : Object
    {
        var cache = GetResourceUnitCache(path);
        if (cache != null)
        {
            if (callback != null) callback(cache.Asset as T);
            return;
        }
        mResourcesLoader.AsyncLoad(path, (asset) =>
        {
            // 同时多个异步加载同时请求时，就都会跳过第一步的缓存判断，这时第一个加载完的就缓存资源
            cache = GetResourceUnitCache(path);
            if (cache == null)
            {
                CacheResourceUnit(path, asset);
            }
            if (callback != null) callback(asset as T);
        });
    }

    /// <summary>
    /// 清缓存
    /// </summary>
    private void ClearCache()
    {
    }

    /// <summary>
    /// 卸载资源
    /// </summary>
    public void Unload(string path, bool destroy = false)
    {
        var unit = GetResourceUnitCache(path, 0);
        if (unit == null || unit.RefCount <= 0) return;
        unit.RefCount--;
        if (unit.RefCount <=0)
        {
            if (destroy)
            {
                mResourceUnitDict.Remove(path);
                mResourcesLoader.Unload(path);
            }
            else
            {
                // 如果不是要马上销毁的先存入没引用列表中
                mNoRefResourceUnitDList.AddToHead(unit);
            }
        }
    }

    /// <summary>
    /// 获取缓存里的资源项
    /// </summary>
    private ResourceUnit GetResourceUnitCache(string path, int refCount = 1)
    {
        ResourceUnit unit;
        if (mResourceUnitDict.TryGetValue(path, out unit))
        {
            unit.RefCount += refCount;
        }
        return unit;
    }

    /// <summary>
    /// 缓存资源
    /// </summary>
    private void CacheResourceUnit(string path, Object asset)
    {
        if (asset == null) return;
        if (mResourceUnitDict.ContainsKey(path))
        {
            Debugger.LogError("Duplicate cache asset {0}", path);
        }
        else
        {
            mResourceUnitDict.Add(path, new ResourceUnit() { Path = path, Asset = asset, RefCount = 1 });
        }
    }

    /// <summary>
    /// 应用退出清除下资源
    /// 主要是编辑器模式下，退出清除资源，查看Profiler不会残留上次的资源
    /// </summary>
    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        Resources.UnloadUnusedAssets();
#endif
    }

    /// <summary>
    /// 测试用例
    /// </summary>
    public void Test()
    {
        // 测试同步加载
        var prefab1 = ResourceManager.Instance.SyncLoad<GameObject>("Assets/GameAssets/Prefabs/ExamplePrefab1.prefab");
        GameObject.Instantiate(prefab1);
        // 测试异步加载
        // ResourceManager.Instance.AsyncLoad<GameObject>("Assets/GameAssets/Prefabs/ExamplePrefab2.prefab", (obj) =>
        // {
        //     GameObject.Instantiate(obj);
        // });
    }
}

public class ResourceUnit
{
    /// <summary>
    /// 资源路径
    /// </summary>
    public string Path;
    /// <summary>
    /// 资源对象
    /// </summary>
    public Object Asset;
    /// <summary>
    /// 引用次数
    /// </summary>
    public int RefCount;
}
