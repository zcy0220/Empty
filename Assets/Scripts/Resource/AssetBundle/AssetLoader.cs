/**
 * 资源加载器
 */

using System;
using Base.Debug;
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
    //public IEnumerator LoadAssetAsync()
    //{
    //    IsLoading = true;
    //    var abBase = AssetBundleManager.Instance.GetAssetBundleBase(Path);
    //    if (abBase == null)
    //    {
    //        Debugger.LogError("No AssetBundleBase is found for the {0}", Path);
    //    }
    //    else
    //    {
    //        var abItem = AssetBundleManager.Instance.GetCacheAssetBundle(abBase.ABName);
    //        if (abItem == null)
    //        {
    //            Debugger.LogError("{0} Cache Error!", abBase.ABName);
    //        }
    //        else
    //        {
    //            var ab = abItem.AssetBundle;
    //            var abRequest = IsSprite() ? ab.LoadAssetAsync<Sprite>(abBase.AssetName) : ab.LoadAssetAsync(abBase.AssetName);
    //            yield return abRequest;
    //            for(var i = 0; i < mCallbackList.Count; i++)
    //            {
    //                mCallbackList[i](abRequest.asset);
    //            }
    //            AssetBundleManager.Instance.AssetLoaderFinished(this);
    //        }
    //    }
    //}

    ///// <summary>
    ///// 判断是否Sprite
    ///// </summary>
    //private bool IsSprite()
    //{
    //    return Path.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) || Path.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase);
    //}

    ///// <summary>
    ///// 判断是否能开始异步加载了，取决于AssetBundle是否完成
    ///// 这里先同步加载下AssetBundle
    ///// </summary>
    //public bool CanLoadAssetAsync()
    //{
    //    if (IsLoading) return false;
    //    var abBase = AssetBundleManager.Instance.GetAssetBundleBase(Path);
    //    AssetBundleManager.Instance.SyncLoadAssetBundle(abBase);
    //    return true;
    //}

    ///// <summary>
    ///// 当请求相同资源时，添加对应回调到回调列表
    ///// </summary>
    //public void AddCallback(Action<UnityEngine.Object> callback)
    //{
    //    if (mCallbackList.Contains(callback)) return;
    //    mCallbackList.Add(callback);
    //}

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
