/**
 * AssetBundle打包编辑
 */

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Base.Debug;

public class AssetBundleEditor
{
    const string ABCONFIGPATH = "Assets/Editor/AssetBundle/ABConfig.asset";
    public static readonly string CreateAssetBundlesPath = Application.streamingAssetsPath;

    [MenuItem("Tools/AssetBundle/Build")]
    public static void Build()
    {
        var abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(ABCONFIGPATH);
        var prefabPathList = AssetDatabase.FindAssets("t:prefab", abConfig.PrefabDirPathList.ToArray());
        for (var i = 0; i < prefabPathList.Length; i++)
        {
            Debugger.Log(AssetDatabase.GUIDToAssetPath(prefabPathList[i]));
        }
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 生成AssetBundles
    /// </summary>
    public static void CreateAssetBundles()
    {
        if (!Directory.Exists(CreateAssetBundlesPath)) Directory.CreateDirectory(CreateAssetBundlesPath);
        BuildPipeline.BuildAssetBundles(CreateAssetBundlesPath, 0, EditorUserBuildSettings.activeBuildTarget);
    }
}
