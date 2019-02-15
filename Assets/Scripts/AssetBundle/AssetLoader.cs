/**
 * 资源加载器
 */

using System;
using System.Collections;
using System.Collections.Generic;

public class AssetLoader : IDisposable, IComparable<AssetLoader>
{
    /// <summary>
    /// 唯一id
    /// </summary>
    private static int mId;
    /// <summary>
    /// 加载成功后的回调列表
    /// </summary>
    private List<Action<UnityEngine.Object>> mCallbackList;
    //------------------------------------------------
    public int Id { get; private set; }
    /// <summary>
    /// 优先级
    /// </summary>
    public int Priority { set; get; }
    /// <summary>
    /// 加载的资源路径
    /// </summary>
    public string Path { set; get; }
    /// <summary>
    /// 是否正在加载
    /// </summary>
    public bool IsLoading { get; private set; }
    
    /// <summary>
    /// 初始化
    /// </summary>
    public AssetLoader()
    {
        mId++;
        Id = mId;
    }

    /// <summary>
    /// 异步加载
    /// </summary>
    /// <returns></returns>
    IEnumerator AsyncLoad()
    {
        IsLoading = true;
        if (string.IsNullOrEmpty(Path)) yield return null;
        var ab = 
        yield return null;
        
    }

    /// <summary>
    /// 优先级相同的情况下，id小的为先请求
    /// </summary>
    public int CompareTo(AssetLoader other)
    {
        return other.Priority == Priority ? Id.CompareTo(other.Id) : other.Priority.CompareTo(Priority);
    }

    /// <summary>
    /// 销毁重置
    /// </summary>
    public void Dispose()
    {
        Priority = 0;
    }
}
