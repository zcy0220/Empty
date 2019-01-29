/**
 * 资源加载器
 */

using System;
using System.Collections;

public abstract class AssetLoader : IDisposable, IComparable<AssetLoader>
{
    /// <summary>
    /// 唯一Id, 主要是排序和删除时候会用到
    /// </summary>
    private static long mId = 0;
    public long Id { get { return mId; } }
    /// <summary>
    /// 当前loader的优先级
    /// </summary>
    public int Priority;
    
    /// <summary>
    /// 构造
    /// </summary>
    protected AssetLoader() { mId++; }

    /// <summary>
    /// 同步加载
    /// </summary>
    public virtual T SyncLoad<T>(string assetPath) where T : UnityEngine.Object
    {
        return null;
    }

    /// <summary>
    /// 异步加载
    /// </summary>
    public virtual IEnumerator AysncLoadAsset()
    {
        yield return null;
    }

    /// <summary>
    /// 优先级比较
    /// </summary>
    public int CompareTo(AssetLoader other)
    {
        return other.Priority == Priority ? Id.CompareTo(other.Id) : other.Priority.CompareTo(Priority);
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose() { }
}
