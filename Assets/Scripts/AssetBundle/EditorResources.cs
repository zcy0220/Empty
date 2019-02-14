/**
 * 编辑模式下资源控制
 * 使用AssetDataBase加载
 */

using System;
using UnityEngine;

public class EditorResources : BaseResources
{
    /// <summary>
    /// 创建ResourceItem
    /// </summary>
    public override ResourceItem CreateResourceItem(string path)
	{
        ResourceItem item = new ResourceItem();
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
#else
        return null;
#endif
    }

    /// <summary>
    /// 编辑器模式下的异步加载就是同步加载
    /// </summary>
    public override void AsyncLoad<T>(string path, Action<UnityEngine.Object> callback)
    {
        var obj = SyncLoad<T>(path);
        callback(obj);
    }

    /// <summary>
    /// 卸载资源
    /// </summary>
    public override void UnloadResource(ResourceItem item)
    {
        if (item == null) return;
        item.Obj = null;
        Resources.UnloadUnusedAssets();
    }
}
