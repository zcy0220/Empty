/**
 * 资源加载管理类
 */

using Base.Common;
using Base.Pool;
using UnityEngine;
using Base.Utils;
using Base.Debug;
using System.Collections.Generic;
using System.Collections;
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
            if (cache.Asset == null)
            {
                cache.Asset = mResourcesLoader.SyncLoad<T>(path);
            }
            cache.RefCount++;
            return cache.Asset as T;
        }
        var obj = mResourcesLoader.SyncLoad<T>(path);
        mResourceUnitDict.Add(path, new ResourceUnit() { Path = path, Asset = obj, RefCount = 1 });
        return obj;
    }

    //public bool Unload()

    /// <summary>
    /// 异步加载
    /// </summary>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    //public void AsyncLoad<T>(string path, System.Action<Object> callback) where T : Object
    //{
    //    if (string.IsNullOrEmpty(path)) return;
    //    var item = GetCacheResourceItem(path);
    //    if (item != null)
    //    {
    //        if (callback != null) callback(item.Obj as T);
    //        return;
    //    }
    //    mResources.AsyncLoad(path, (obj) =>
    //    {
    //        item = GetCacheResourceItem(path);
    //        if (item == null)
    //        {
    //            item = mResources.CreateResourceItem(path);
    //            CacheResourceItem(path, obj, item);
    //        }
    //        if (callback != null) callback(obj as T);
    //    });
    //}

    ///// <summary>
    ///// 不需要实例化的资源卸载
    ///// </summary>
    //public bool ReleaseResource(Object obj, bool destroy = false)
    //{
    //    if (obj == null) return false;
    //    ResourceItem item = null;
    //    foreach(var res in ResourceDict.Values)
    //    {
    //        if (res.Guid == obj.GetInstanceID())
    //        {
    //            item = res;
    //        }
    //    }
    //    if (item == null)
    //    {
    //        Debugger.LogError("ResourceDict not exist ResourceItem: {0}", obj.name);
    //        return false;
    //    }
    //    item.RefCount--;
    //    RecycleResourceItem(item, destroy);
    //    return true;
    //}

    ///// <summary>
    ///// 缓存资源项
    ///// </summary>
    //private void CacheResourceItem(string path, Object obj, ResourceItem item)
    //{
    //    ClearCache();

    //    if (item == null)
    //    {
    //        Debugger.LogError("{0} ResourceItem is null!", path);
    //        return;
    //    }
    //    if (obj == null)
    //    {
    //        Debugger.LogError("Resource load fail: {0}", path);
    //        return;
    //    }
    //    item.Obj = obj;
    //    item.Guid = obj.GetInstanceID();
    //    item.LastUseTime = Time.realtimeSinceStartup;
    //    item.RefCount++;
    //    if (!ResourceDict.ContainsKey(path)) ResourceDict.Add(path, item);
    //}

    ///// <summary>
    ///// 清缓存
    ///// </summary>
    //private void ClearCache()
    //{
    //}

    ///// <summary>
    ///// 回收资源
    ///// </summary>
    //private void RecycleResourceItem(ResourceItem item, bool destroy = false)
    //{
    //    if (item == null || item.RefCount > 0) return;
    //    if (!destroy)
    //    {
    //        mNoRefResourceMapList.AddToHead(item);
    //        return;
    //    }
    //    if (!ResourceDict.Remove(item.Path)) return;
    //    mResources.UnloadResource(item);
    //}

    /// <summary>
    /// 获取缓存里的资源项
    /// </summary>
    private ResourceUnit GetResourceUnitCache(string path)
    {
        ResourceUnit unit;
        mResourceUnitDict.TryGetValue(path, out unit);
        return unit;
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
