/**
 * AssetBundle打包编辑
 */

using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class AssetBundleEditor
{
    /// <summary>
    /// AssetBundle配置文件路径
    /// </summary>
    const string ABCONFIGPATH = "Assets/Editor/AssetBundle/ABConfig.asset";
    /// <summary>
    /// 生成AssetBundle路径
    /// </summary>
    static readonly string CreateAssetBundlesPath = Application.streamingAssetsPath;
    /// <summary>
    /// 所有文件夹或文件路径列表
    /// </summary>
    private static List<string> mAllPathList = new List<string>();
    /// <summary>
    /// 所有prefab对应的剔除冗余之后的依赖列表
    /// </summary>
    private static Dictionary<string, List<string>> mPrefabDependsDict = new Dictionary<string, List<string>>();

    [MenuItem("Tools/AssetBundle/Build")]
    public static void Build()
    {
        mAllPathList.Clear();
        mPrefabDependsDict.Clear();

        var abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(ABCONFIGPATH);

        foreach (var dirPath in abConfig.DirectoryPathDict)
        {
            mAllPathList.Add(dirPath.Value);
        }

        var prefabPathList = AssetDatabase.FindAssets("t:prefab", abConfig.PrefabDirPathList.ToArray());
        for (var i = 0; i < prefabPathList.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(prefabPathList[i]);
            EditorUtility.DisplayProgressBar("查找Prefab", "Path: " + path, i * 1.0f / prefabPathList.Length);
            if (!ContainPath(path))
            {
                var depends = AssetDatabase.GetDependencies(path);
                var dependsList = new List<string>();
                for (var j = 0; j < depends.Length; j++)
                {
                    if (!ContainPath(depends[j]) && !depends[j].EndsWith(".cs"))
                    {
                        mAllPathList.Add(depends[j]);
                        dependsList.Add(depends[j]);
                    }
                }
                mPrefabDependsDict.Add(path, dependsList);
            }
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 是否包含路径
    /// </summary>
    private static bool ContainPath(string path)
    {
        foreach(var PATH in mAllPathList)
        {
            if (path == PATH || path.Contains(PATH))
            {
                return true;
            }
        }
        return false;
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
