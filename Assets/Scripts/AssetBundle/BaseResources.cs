/**
 * 资源控制基类
 */

using UnityEngine;

public abstract class BaseResources
{
    /// <summary>
    /// 创建ResourceItem
    /// </summary>
    public virtual ResourceItem CreateResourceItem(string path) { return null; }

    /// <summary>
    /// 同步加载资源
    /// </summary>
    public virtual T SyncLoad<T>(string path) where T : Object { return null; }
    
    /// <summary>
    /// 异步加载资源
    /// </summary>
    public virtual void AsyncLoad(string path, System.Action<Object> callback) { }

    /// <summary>
    /// 卸载资源
    /// </summary>
    public virtual void UnloadResource(ResourceItem item) {}
}
