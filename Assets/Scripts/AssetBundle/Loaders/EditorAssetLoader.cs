/**
 * 编辑器的加载方式
 * 使用AssetDataBase
 */

using UnityEditor;
using Base.Debug;

public class EditorAssetLoader : AssetLoader
{
    /// <summary>
    /// 编辑器下的同步加载
    /// </summary>
    public override T SyncLoad<T>(string assetPath)
    {
        #if UNITY_EDITOR
            //var assetImporter = AssetImporter.GetAtPath(assetPath);
            //if (assetImporter == null) return null;
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        #else
            return null;
        #endif
    }
}
