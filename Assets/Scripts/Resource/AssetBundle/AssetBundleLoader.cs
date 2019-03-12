/**
 * AB包异步加载器
 */

using System;
using UnityEngine;
using Base.Utils;
using System.Collections;

public class AssetBundleLoader : IDisposable
{
    /// <summary>
    /// 加载的AB包名
    /// </summary>
    public string AssetBundleName { set; get; }
    /// <summary>
    /// 是否正在加载
    /// </summary>
    public bool IsLoading { get; private set; }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <returns></returns>
    public IEnumerator AsyncLoadAssetBundle()
    {
        IsLoading = true;
        var path = PathUtil.GetLocalAssetBundleFilePath(AssetBundleName);
        var abCreateRequest = AssetBundle.LoadFromFileAsync(path);
        yield return abCreateRequest;
        AssetBundleManager.Instance.LoadAssetBundleFinished(this, abCreateRequest.assetBundle);
    }

    /// <summary>
    /// 销毁重置
    /// </summary>
    public void Dispose()
    {
        IsLoading = false;
        AssetBundleName = string.Empty;
    }
}