/**
 * 编辑模式下资源控制
 * 使用AssetDataBase加载
 */

using UnityEngine;

public class EditorResourceLoader : BaseResourceLoader
{
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
    /// 编辑器模式下的异步加载就是同步加载模拟
    /// </summary>
    public override void AsyncLoad(string path, System.Action<Object> callback)
    {
        Object obj = IsSprite(path) ? SyncLoad<Sprite>(path) : SyncLoad<Object>(path);
        callback(obj);
    }

    /// <summary>
    /// 判断是否Sprite
    /// </summary>
    private bool IsSprite(string path)
    {
        return path.EndsWith(".png") || path.EndsWith(".jpg");
    }
}
