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
        var md5 = FileUtil.GetMD5HashFromFile(path);
        var abBase = AssetBundleManager.Instance.GetAssetBundleBase(md5);
        item.MD5 = md5;
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
        var md5 = FileUtil.GetMD5HashFromFile(path);
        var abBase = AssetBundleManager.Instance.GetAssetBundleBase(md5);
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
        if (item.ABDependList != null)
        {
            for (var i = 0; i < item.ABDependList.Count; i++)
            {
                AssetBundleManager.Instance.UnloadAssetBundle(item.ABDependList[i]);
            }
        }
        AssetBundleManager.Instance.UnloadAssetBundle(item.ABName);
    }
}
