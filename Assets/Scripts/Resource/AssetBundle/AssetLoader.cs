/**
 * 资源异步加载器
 */

using System;
using UnityEngine;
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
    /// 异步加载资源
    /// </summary>
    /// <returns></returns>
    public IEnumerator AsyncLoadAsset()
    {
        IsLoading = true;
        var unit = AssetBundleManager.Instance.GetAssetBundleUnit(Path);
        AssetBundleManager.Instance.RefAllAssetBundles(Path);
        var assetBundle = unit.AssetBundle;
        var abRequest = IsSprite() ? assetBundle.LoadAssetAsync<Sprite>(Path) : assetBundle.LoadAssetAsync(Path);
        yield return abRequest;
        for(var i = 0; i < mCallbackList.Count; i++)
        {
            mCallbackList[i](abRequest.asset);
        }
        AssetBundleManager.Instance.LoadAssetFinished(this);
    }

    /// <summary>
    /// 判断是否Sprite
    /// </summary>
    private bool IsSprite()
    {
        return Path.EndsWith(".png", StringComparison.OrdinalIgnoreCase) || Path.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 判断是否能开始异步加载了，取决于AssetBundle是否完成
    /// 这里先同步加载下AssetBundle
    /// </summary>
    public bool CanLoadAssetAsync()
    {
        if (IsLoading) return false;
        return AssetBundleManager.Instance.CheckAllAssetBundleLoaded(Path);
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
        IsLoading = false;
        Path = string.Empty;
        mCallbackList.Clear();
    }
}
