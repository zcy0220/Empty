/**
 * AssetBundle打包编辑
 */

using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class AssetBundleEditor
{
    public const string ABCONFIGPATH = "Assets/Editor/AssetBundle/ABConfig.asset";
    public static string CreateAssetBundlesPath = Application.streamingAssetsPath;
    //==============================================================================
    private static List<string> mAllFilePathList = new List<string>();

    [MenuItem("Tools/AssetBundle/Build")]
    public static void Build()
    {
        mAllFilePathList.Clear();

        var abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(ABCONFIGPATH);

        foreach (var dirPath in abConfig.DirectoryPathDict)
        {
            var dirInfo = new DirectoryInfo(dirPath.Value);
            var files = dirInfo.GetFiles();
            for (var i = 0; i < files.Length; i++)
            {
                var file = files[i];
                if (file.Extension != ".meta")
                {
                    Debug.Log(file);
                }
            }
            //mAllPathAB.Add(dirPath.Value);
        }

        var prefabPathList = AssetDatabase.FindAssets("t:prefab", abConfig.PrefabDirPathList.ToArray());
        for (var i = 0; i < prefabPathList.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(prefabPathList[i]);
            EditorUtility.DisplayProgressBar("查找Prefab", "Path: " + path, i * 1.0f / prefabPathList.Length);
            var depends = AssetDatabase.GetDependencies(path);
            for (var j = 0; j < depends.Length; j++)
            {
                //Debug.Log(depends[j]);
            }
        }
        EditorUtility.ClearProgressBar();
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
