/**
 * 编辑模式下资源控制
 * 使用AssetDataBase加载
 */

using Base.Utils;
using UnityEngine;

public class EditorResources : BaseResources
{
    /// <summary>
    /// 创建ResourceItem
    /// </summary>
    public override ResourceItem CreateResourceItem(string path)
	{
        ResourceItem item = new ResourceItem();
        var md5 = FileUtil.GetMD5HashFromFile(path);
        item.MD5 = md5;
        item.Path = path;
        return item;
	}


    /// <summary>
    /// 同步加载资源
    /// </summary>
    public override T SyncLoad<T>(string path)
    {
#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
#endif
    }

    /// <summary>
    /// 卸载资源
    /// </summary>
    public override void UnloadResource(ResourceItem item)
    {
        if (item == null) return;
        Resources.UnloadAsset(item.Obj);
        item.Obj = null;
    }
}
