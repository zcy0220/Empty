/**
 * 编辑器模式下的加载方式
 * 使用AssetDataBase
 */

using UnityEditor;

public class EditorAssetLoader : AssetLoader
{
    /// <summary>
    /// 编辑器下的同步加载
    /// </summary>
    public override T SyncLoad<T>(string path)
    {
        return AssetDatabase.LoadAssetAtPath<T>(path);
    }
}
