/**
 * AssetBundle打包编辑
 * 文件夹以配置文件中设好的ABName为名
 * prefab以去掉后缀的路径为名
 */

using System.IO;
using UnityEditor;
using UnityEngine;
using Base.Debug;
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
    /// 所有文件夹路径及ABName
    /// </summary>
    private static Dictionary<string, string> mDirectoryPathDict;
    /// <summary>
    /// 所有prefab对应的剔除冗余之后的依赖列表
    /// </summary>
    private static Dictionary<string, List<string>> mPrefabDependsDict = new Dictionary<string, List<string>>();

    [MenuItem("Tools/AssetBundle/Build")]
    public static void Build()
    {
        FilterAndDependPaths();
        SetAllAssetBundleNames();
        BuildAssetBundle();
        ClearAssetBundleNames();
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
    }

    /// <summary>
    /// 过滤和依赖path
    /// </summary>
    private static void FilterAndDependPaths()
    {
        mAllPathList.Clear();
        mPrefabDependsDict.Clear();
        var abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(ABCONFIGPATH);
        mDirectoryPathDict = abConfig.DirectoryPathDict;
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
                mPrefabDependsDict.Add(path.Replace(".prefab", "").Replace("/", "_"), dependsList);
            }
        }
    }

    /// <summary>
    /// 设置所有的ABName
    /// </summary>
    private static void SetAllAssetBundleNames()
    {
        foreach (var dir in mDirectoryPathDict)
        {
            SetAssetBundleName(dir.Key, dir.Value);
        }

        foreach (var prefab in mPrefabDependsDict)
        {
            SetAssetBundleName(prefab.Key, prefab.Value);
        }
    }

    /// <summary>
    /// 设置AssetBundle包名
    /// </summary>
    private static void SetAssetBundleName(string name, string path)
    {
        var assetImport = AssetImporter.GetAtPath(path);
        if (assetImport != null)
        {
            assetImport.assetBundleName = name;
        }
        else
        {
            Debugger.LogError("{0} not exist!", path);
        }
    }

    /// <summary>
    /// 设置AssetBundle包名
    /// </summary>
    private static void SetAssetBundleName(string name, List<string> paths)
    {
        foreach(var path in paths)
        {
            SetAssetBundleName(name, path);
        }
    }

    /// <summary>
    /// 清除所有ABName
    /// </summary>
    private static void ClearAssetBundleNames()
    {
        var abNames = AssetDatabase.GetAllAssetBundleNames();
        for (var i = 0; i < abNames.Length; i++)
        {
            AssetDatabase.RemoveAssetBundleName(abNames[i], true);
        }
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
    /// 删除没用的AB包
    /// </summary>
    private static void ClearUnUseAssetBundle()
    {
        var abNames = AssetDatabase.GetAllAssetBundleNames();
        var direction = new DirectoryInfo(CreateAssetBundlesPath);
        var files = direction.GetFiles();
        for (var i = 0; i < files.Length; i++)
        {
            if (ContainABName(files[i].Name, abNames) || files[i].Name.EndsWith(".meta"))
            {
                continue;
            }
            else
            {
                if (File.Exists(files[i].FullName))
                {
                    File.Delete(files[i].FullName);
                }
            }
        }
    }

    /// <summary>
    /// Contains the ABN ame.
    /// </summary>
    private static bool ContainABName(string name, string[] strs)
    {
        for (var i = 0; i < strs.Length; i++)
        {
            if (strs[i] == name)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 构建AssetBundle
    /// </summary>
    private static void BuildAssetBundle()
    {
        var abNames = AssetDatabase.GetAllAssetBundleNames();
        var resPathDict = new Dictionary<string, string>(); //key为全路径，value为包名
        for (var i = 0; i < abNames.Length; i++)
        {
            var allBundlePaths = AssetDatabase.GetAssetPathsFromAssetBundle(abNames[i]);
            for (var j = 0; j < allBundlePaths.Length; j++)
            {
                resPathDict.Add(allBundlePaths[j], abNames[i]);
            }
        }
        ClearUnUseAssetBundle();
        CreateAssetBundles();
    }

    /// <summary>
    /// 生成AssetBundles
    /// </summary>
    public static void CreateAssetBundles()
    {
        if (!Directory.Exists(CreateAssetBundlesPath)) Directory.CreateDirectory(CreateAssetBundlesPath);
        BuildPipeline.BuildAssetBundles(CreateAssetBundlesPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
    }
}
