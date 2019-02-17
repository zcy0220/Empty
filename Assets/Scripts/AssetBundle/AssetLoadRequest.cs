/**
 * 资源加载请求
 */

using System;

public class AssetLoadRequest : IDisposable, IComparable<AssetLoadRequest>
{
    /// <summary>
    /// 唯一id
    /// </summary>
    private static int mId;
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
    /// 对调
    /// </summary>
    public Action<UnityEngine.Object> Callback;

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        mId++;
        Id = mId;
    }

    /// <summary>
    /// 优先级相同的情况下，id小的为先请求
    /// </summary>
    public int CompareTo(AssetLoadRequest other)
    {
        return other.Priority == Priority ? Id.CompareTo(other.Id) : other.Priority.CompareTo(Priority);
    }

    /// <summary>
    /// 销毁重置
    /// </summary>
    public void Dispose()
    {
        Id = 0;
        Callback = null;
    }
}
