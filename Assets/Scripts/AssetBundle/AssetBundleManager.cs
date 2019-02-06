/**
 * AssetBundle资源管理
 */

using Base.Common;
using UnityEngine;
using Base.Debug;
using System.IO;
using System.Collections.Generic;
using Base.Utils;
using System.Runtime.Serialization.Formatters.Binary;

public class AssetBundleManager : Singleton<AssetBundleManager>
{
    /// <summary>
    /// 配置文件路径
    /// </summary>
    private readonly string ASSETBUNDLECONFIGPATH = Application.streamingAssetsPath + "/assetbundleconfig";
    /// <summary>
    /// 存一份以md5为key的资源表
    /// </summary>
    private Dictionary<string, ResourceItem> mResourceItemDict = new Dictionary<string, ResourceItem>();
    /// <summary>
    /// 存储加载的AssetBundleItem
    /// </summary>
    private Dictionary<string, AssetBundleItem> mAssetBundleItemDict = new Dictionary<string, AssetBundleItem>();
    /// <summary>
    /// AssetBundleItem对应的类对象池
    /// </summary>
    private ClassObjectPool<AssetBundleItem> mAssetBundleItemPool = ObjectManager.Instance.GetOrCreateClassPool<AssetBundleItem>();

    /// <summary>
    /// 加载AssetBundle配置文件
    /// </summary>
    public bool LoadAssetBundleConfig()
    {
        mResourceItemDict.Clear();
        mAssetBundleItemDict.Clear();
        var assetBundle = AssetBundle.LoadFromFile(ASSETBUNDLECONFIGPATH);
        var textAsset = assetBundle.LoadAsset<TextAsset>("assetbundleconfig");
        if (textAsset == null)
        {
            Debugger.LogError("AssetBundleConfig is not exist!");
            return false;
        }
        var stream = new MemoryStream(textAsset.bytes);
        var formatter = new BinaryFormatter();
        var assetBundleConfig = (AssetBundleConfig)formatter.Deserialize(stream);
        stream.Close();
        for (var i = 0; i < assetBundleConfig.AssetBundleList.Count; i++)
        {
            var abBase = assetBundleConfig.AssetBundleList[i];
            var md5 = abBase.MD5;
            if (mResourceItemDict.ContainsKey(md5))
            {
                Debugger.LogError("Duplicate MD5! AssetName:{0}, ABName:{1}", abBase.AssetName, abBase.ABName);
            }
            else
            {
                var item = new ResourceItem();
                item.MD5 = abBase.MD5;
                item.ABName = abBase.ABName;
                item.AssetName = abBase.AssetName;
                item.ABDependList = abBase.ABDependList;
                mResourceItemDict.Add(md5, item);
            }
        }
        return true;
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    public ResourceItem LoadResourceAssetBundle(string md5)
    {
        ResourceItem item = null;
        if (!mResourceItemDict.TryGetValue(md5, out item) || item == null)
        {
            Debugger.LogError("Can not find md5 {0}!", md5);
            return item;
        }
        if (item.AssetBundle != null) return item;
        item.AssetBundle = LoadAssetBundle(item.ABName);
        if (item.ABDependList != null)
        {
            for (var i = 0; i < item.ABDependList.Count; i++)
            {
                LoadAssetBundle(item.ABDependList[i]);
            }
        }
        return item;
    }

    /// <summary>
    /// 卸载资源
    /// </summary>
    public void UnloadResourceAssetBundle(ResourceItem item)
    {
        if (item == null) return;
        if (item.ABDependList != null)
        {
            for (var i = 0; i < item.ABDependList.Count; i++)
            {
                UnloadAssetBundle(item.ABDependList[i]);
            }
        }
        UnloadAssetBundle(item.ABName);
    }

    /// <summary>
    /// 根据AB包名加载AssetBundle
    /// </summary>
    private AssetBundle LoadAssetBundle(string name)
    {
        AssetBundleItem item = null;
        if (!mAssetBundleItemDict.TryGetValue(name, out item))
        {
            AssetBundle assetBundle = null;
            var path = StringUtil.Concat(Application.streamingAssetsPath, "/", name);
            if (File.Exists(path)) assetBundle = AssetBundle.LoadFromFile(path);
            if (assetBundle == null) Debugger.LogError("Load AssetBundle Error: " + name);
            item = mAssetBundleItemPool.Spawn();
            item.AssetBundle = assetBundle;
            item.RefCount++;
            mAssetBundleItemDict.Add(name, item);
        }
        else
        {
            item.RefCount++;
        }
        return item.AssetBundle;
    }

    /// <summary>
    /// 卸载AssetBundle
    /// </summary>
    private void UnloadAssetBundle(string name)
    {
        AssetBundleItem item = null;
        if (mAssetBundleItemDict.TryGetValue(name, out item) && item != null)
        {
            item.RefCount--;
            if (item.RefCount <= 0 && item.AssetBundle != null)
            {
                item.AssetBundle.Unload(true);
                item.Reset();
                mAssetBundleItemPool.Recycle(item);
                mAssetBundleItemDict.Remove(name);
            }
        }
    }

    /// <summary>
    /// 获取ResourceItem
    /// </summary>
    public ResourceItem GetResourceItem(string name)
    {
        if (mResourceItemDict.ContainsKey(name)) return mResourceItemDict[name];
        return null;
    }
}
