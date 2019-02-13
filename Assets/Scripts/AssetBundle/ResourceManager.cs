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

public class ResourceManager : MonoSingleton<ResourceManager>
{
    /// <summary>
    /// 对应平台模式下的资源控制
    /// </summary>
    private BaseResources mResources;
    /// <summary>
    /// 缓存引用计数为0的资源，达到缓存上限时情掉最早没用的资源
    /// </summary>
    private ResourceMapList<ResourceItem> mNoRefResourceMapList = new ResourceMapList<ResourceItem>();
    /// <summary>
    /// 缓存使用的资源列表
    /// </summary>
    public Dictionary<string, ResourceItem> ResourceDict { get; private set; } = new Dictionary<string, ResourceItem>();

    /// <summary>
    /// 初始化
    /// </summary>
    private ResourceManager()
	{
        if (AppConfig.UseAssetBundle)
        {
            mResources = new AssetBundleResources();
        }
        else
        {
            mResources = new EditorResources();
        }
	}

	/// <summary>
	/// 同步加载
	/// </summary>
	public T SyncLoad<T>(string path) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(path)) return null;
        var item = GetCacheResourceItem(path);
        if (item != null) return item.Obj as T;
        item = mResources.CreateResourceItem(path);
        var obj = mResources.SyncLoad<T>(path);
        CacheResourceItem(path, obj, item);
        return obj;
    }

    /// <summary>
    /// 不需要实例化的资源卸载
    /// </summary>
    public bool ReleaseResource(Object obj, bool destroy = false)
    {
        if (obj == null) return false;
        ResourceItem item = null;
        foreach(var res in ResourceDict.Values)
        {
            if (res.Guid == obj.GetInstanceID())
            {
                item = res;
            }
        }
        if (item == null)
        {
            Debugger.LogError("ResourceDict not exist ResourceItem: {0}", obj.name);
            return false;
        }
        item.RefCount--;
        RecycleResourceItem(item, destroy);
        return true;
    }

    /// <summary>
    /// 缓存资源项
    /// </summary>
    private void CacheResourceItem(string path, Object obj, ResourceItem item)
    {
        ClearCache();

        if (item == null)
        {
            Debugger.LogError("{0} ResourceItem is null!", path);
            return;
        }
        if (obj == null)
        {
            Debugger.LogError("Resource load fail: {0}", path);
            return;
        }
        item.Obj = obj;
        item.Guid = obj.GetInstanceID();
        item.LastUseTime = Time.realtimeSinceStartup;
        item.RefCount++;
        if (!ResourceDict.ContainsKey(path)) ResourceDict.Add(path, item);
    }

    /// <summary>
    /// 清缓存
    /// </summary>
    private void ClearCache()
    {
    }

    /// <summary>
    /// 回收资源
    /// </summary>
    private void RecycleResourceItem(ResourceItem item, bool destroy = false)
    {
        if (item == null || item.RefCount > 0) return;
        if (!destroy)
        {
            mNoRefResourceMapList.AddToHead(item);
            return;
        }
        if (!ResourceDict.Remove(item.Path)) return;
        mResources.UnloadResource(item);
    }

    /// <summary>
    /// 获取缓存里的资源项
    /// </summary>
    private ResourceItem GetCacheResourceItem(string path)
    {
        ResourceItem item = null;
        if (ResourceDict.TryGetValue(path, out item) && item != null)
        {
            item.RefCount++;
            item.LastUseTime = Time.realtimeSinceStartup;
        }
        return item;
    }
}
