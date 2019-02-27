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
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// 资源单项信息
/// </summary>
public class AssetItemInfo
{
    /// <summary>
    /// 自身被依赖的列表
    /// </summary>
    public List<string> BeDependentOnList = new List<string>();
    /// <summary>
    /// 要设置的AB包名
    /// </summary>
    public string ABName = string.Empty;
}

public class AssetBundleEditor
{
    /// <summary>
    /// 资源根目录
    /// </summary>
    private const string ASSETROOT = "Assets/GameAssets";
    /// <summary>
    /// 生成AssetBundle路径
    /// </summary>
    private static readonly string ASSETBUNDLESPATH = Application.streamingAssetsPath;
    /// <summary>
    /// key为资源路径
    /// </summary>
    private static Dictionary<string, AssetItemInfo> mAssetItemInfoDict = new Dictionary<string, AssetItemInfo>();

    public static void Build()
    {
        TraverseGameAssets();
        //SetAllAssetBundleNames();
        //BuildAssetBundle();
        //ClearAssetBundleNames();
        //AssetDatabase.Refresh();
        //EditorUtility.ClearProgressBar();
    }

    /// <summary>
    /// 遍历根目录下的所有资源，设置依赖信息
    /// </summary>
    private static void TraverseGameAssets()
    {
        mAssetItemInfoDict.Clear();

        // 非递归深度优先
        var stack = new Stack<string>();
        stack.Push(ASSETROOT);
        while(stack.Count > 0)
        {
            var dirPath = stack.Pop();
            var dirInfo = new DirectoryInfo(dirPath);
            var childDirs = dirInfo.GetDirectories();
            if (childDirs != null)
            {
                for (var i = 0; i < childDirs.Length; i++)
                {
                    stack.Push(childDirs[i].FullName);
                }
            }
            var files = dirInfo.GetFiles();
            for (var i = 0; i < files.Length; i++)
            {
                var fileInfo = files[i];
                if (fileInfo.Extension != ".meta")
                {
                    var fullPath = fileInfo.FullName.Replace("\\", "/");
                    var path = fullPath.Substring(fullPath.IndexOf(ASSETROOT));
                    AssetItemInfo assetItem;
                    if (!mAssetItemInfoDict.TryGetValue(path, out assetItem))
                    {
                        assetItem = new AssetItemInfo();
                        mAssetItemInfoDict.Add(path, assetItem);
                    }
                    assetItem.ABName = path.ToLower();
                    var depends = AssetDatabase.GetDependencies(path);
                    for(var j = 0; j < depends.Length; j++)
                    {
                        //if ()
                    }
                }
            }
        }

        //mAllPathList.Clear();
        //mPrefabDependsDict.Clear();
        //mVaildPathList.Clear();
        //var abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(ABCONFIGPATH);
        //mDirectoryPathDict = abConfig.DirectoryPathDict;
        //foreach (var dirPath in abConfig.DirectoryPathDict)
        //{
        //    mAllPathList.Add(dirPath.Value);
        //    mVaildPathList.Add(dirPath.Value);
        //}
        //var prefabPathList = AssetDatabase.FindAssets("t:prefab", abConfig.PrefabDirPathList.ToArray());
        //for (var i = 0; i < prefabPathList.Length; i++)
        //{
        //    var path = AssetDatabase.GUIDToAssetPath(prefabPathList[i]);
        //    mVaildPathList.Add(path);
        //    EditorUtility.DisplayProgressBar("查找Prefab", "Path: " + path, i * 1.0f / prefabPathList.Length);
        //    if (!ContainPath(path))
        //    {
        //        var depends = AssetDatabase.GetDependencies(path);
        //        var dependsList = new List<string>();
        //        for (var j = 0; j < depends.Length; j++)
        //        {
        //            if (!ContainPath(depends[j]) && !depends[j].EndsWith(".cs"))
        //            {
        //                mAllPathList.Add(depends[j]);
        //                dependsList.Add(depends[j]);
        //            }
        //        }
        //        mPrefabDependsDict.Add(path.Replace(".prefab", "").Replace("/", "_"), dependsList);
        //    }
        //}
    }

    /// <summary>
    /// 设置所有的ABName
    /// </summary>
    private static void SetAllAssetBundleNames()
    {
        //foreach (var dir in mDirectoryPathDict)
        //{
        //    SetAssetBundleName(dir.Key, dir.Value);
        //}

        //foreach (var prefab in mPrefabDependsDict)
        //{
        //    SetAssetBundleName(prefab.Key, prefab.Value);
        //}
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
    /// 1.Assets/GameAssets/Shaders -- Assets/GameAssets/Shaders相同
    /// 2.Assets/GameAssets/Shaders/ExampleShader.shader -- Assets/GameAssets/Shaders 包含
    /// 3.Assets/GameAssets/ShadersTest -- Assets/GameAssets/Shaders 不包含
    /// </summary>
    private static bool ContainPath(string path)
    {
        //foreach(var PATH in mAllPathList)
        //{
        //    if (path == PATH || (path.Contains(PATH) && path.Replace(PATH, "")[0] == '/'))
        //    {
        //        return true;
        //    }
        //}
        return false;
    }

    /// <summary>
    /// 删除没用的AB包
    /// </summary>
    private static void ClearUnUseAssetBundle()
    {
        var abNames = AssetDatabase.GetAllAssetBundleNames();
        if (!Directory.Exists(ASSETBUNDLESPATH)) Directory.CreateDirectory(ASSETBUNDLESPATH);
        var direction = new DirectoryInfo(ASSETBUNDLESPATH);
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
                if (!IsVaildPath(allBundlePaths[j])) continue;
                resPathDict.Add(allBundlePaths[j], abNames[i]);
            }
        }
        ClearUnUseAssetBundle();
        //WriteAssetBundleConfig(resPathDict);
        CreateAssetBundles();
    }

    /// <summary>
    /// 写配置文件
    /// </summary>
    private static void WriteAssetBundleConfig(Dictionary<string, string> resPathDict)
    {
        var assetBundleConfig = new AssetBundleConfig();
        assetBundleConfig.AssetBundleList = new List<AssetBundleBase>();
        foreach(var path in resPathDict.Keys)
        {
            var abBase = new AssetBundleBase();
            abBase.Path = path;
            abBase.ABName = resPathDict[path];
            abBase.AssetName = path.Remove(0, path.LastIndexOf("/") + 1);
            abBase.ABDependList = new List<string>();
            var dependencies = AssetDatabase.GetDependencies(path);
            for (var i = 0; i < dependencies.Length; i++)
            {
                var tempPath = dependencies[i];
                if (tempPath == path || tempPath.EndsWith(".cs")) continue;
                if (resPathDict.ContainsKey(tempPath))
                {
                    var abName = resPathDict[tempPath];
                    if (abName == resPathDict[path]) continue;
                    if (!abBase.ABDependList.Contains(abName)) abBase.ABDependList.Add(abName);
                }
            }
            assetBundleConfig.AssetBundleList.Add(abBase);
        }

        //if (File.Exists(ASSETBUNDLECONFIGXMLPATH)) File.Delete(ASSETBUNDLECONFIGXMLPATH);
        //var fileStream = new FileStream(ASSETBUNDLECONFIGXMLPATH, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        //var sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        //var xs = new XmlSerializer(assetBundleConfig.GetType());
        //xs.Serialize(sw, assetBundleConfig);
        //sw.Close();
        //fileStream.Close();

        //var fs = new FileStream(ASSETBUNDLECONFIGBYTESPATH, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        //var bf = new BinaryFormatter();
        //bf.Serialize(fs, assetBundleConfig);
        //fs.Close();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 判断是否为有效路径
    /// </summary>
    private static bool IsVaildPath(string path)
    {
        //for (var i = 0; i < mVaildPathList.Count; i++)
        //{
        //    if (path.Contains(mVaildPathList[i]))
        //        return true;
        //}
        return false;
    }

    /// <summary>
    /// 生成AssetBundles
    /// </summary>
    private static void CreateAssetBundles()
    {
        if (!Directory.Exists(ASSETBUNDLESPATH)) Directory.CreateDirectory(ASSETBUNDLESPATH);
        BuildPipeline.BuildAssetBundles(ASSETBUNDLESPATH, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
    }
}
