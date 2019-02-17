/**
 * 资源加载器
 */

using System;
using System.Collections;
using System.Collections.Generic;

public class AssetLoader : IDisposable
{
    /// <summary>
    /// 加载成功后的回调列表
    /// </summary>
    private List<Action<UnityEngine.Object>> mCallbackList = new List<Action<UnityEngine.Object>>();
    //------------------------------------------------
    /// <summary>
    /// 加载的资源路径
    /// </summary>
    public string Path { set; get; }
    /// <summary>
    /// 是否正在加载
    /// </summary>
    public bool IsLoading { get; private set; }

    /// <summary>
    /// 异步加载
    /// </summary>
    /// <returns></returns>
    IEnumerator AsyncLoad()
    {
        IsLoading = true;
        if (string.IsNullOrEmpty(Path)) yield return null;
        yield return null;
        
    }

    /// <summary>
    /// 当请求相同资源时，添加对应回调到回调列表
    /// </summary>
    public void AddCallback(Action<UnityEngine.Object> callback)
    {
        if (mCallbackList.Contains(callback)) return;
        mCallbackList.Add(callback);
    }

    /// <summary>
    /// 销毁重置
    /// </summary>
    public void Dispose()
    {
        mCallbackList.Clear();
    }
}
