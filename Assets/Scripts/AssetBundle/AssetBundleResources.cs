/**
 * AB资源控制
 */

using Base.Utils;
using Base.Debug;

public class AssetBundleResources : BaseResources
{
    /// <summary>
    /// 创建ResourceItem
    /// </summary>
    public override ResourceItem CreateResourceItem(string path)
    {
        ResourceItem item = new ResourceItem();
        var abBase = AssetBundleManager.Instance.GetAssetBundleBase(path);
        item.Path = path;
        item.ABName = abBase.ABName;
        item.AssetName = abBase.AssetName;
        item.ABDependList = abBase.ABDependList;
        return item;
    }

    /// <summary>
    /// 同步加载资源
    /// </summary>
    public override T SyncLoad<T>(string path)
    {
        var abBase = AssetBundleManager.Instance.GetAssetBundleBase(path);
        if (abBase != null)
        {
            var ab = AssetBundleManager.Instance.LoadAssetBundle(abBase);
            if (ab != null)
            {
                return ab.LoadAsset<T>(abBase.AssetName);
            }
        }
        else
        {
            Debugger.LogError("No AssetBundleBase is found for the {0}", path);
        }
        return null;
    }

    /// <summary>
    /// 卸载资源
    /// </summary>
    public override void UnloadResource(ResourceItem item)
    {
        if (item == null) return;
        item.Obj = null;
        AssetBundleManager.Instance.UnloadAssetBundleByPath(item.Path);
    }
}
