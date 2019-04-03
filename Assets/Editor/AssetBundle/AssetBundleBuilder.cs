/**
 * AssetBundle打包构建
 */

using System.IO;
using UnityEditor;
using System.Collections.Generic;
using Base.Debug;

namespace Assets.Editor.AssetBundle
{
    public class AssetBundleBuilder
    {
        /// <summary>
        /// AssetBundle文件后缀
        /// </summary>
        public const string ASSETBUNDLEEX = ".assetbundle";
        /// <summary>
        /// 资源依赖映射
        /// </summary>
        private static Dictionary<string, AssetItem> mAssetItemDict = new Dictionary<string, AssetItem>();
        /// <summary>
        /// SpriteAtlas依赖映射
        /// </summary>
        private static Dictionary<string, string> mSpriteAtlasDict = new Dictionary<string, string>();
        /// <summary>
        /// 构建的目标平台
        /// </summary>
        private static BuildTarget mBuildTarget = BuildTarget.NoTarget;

        [MenuItem("Tools/AssetBundle/Build")]
        public static void Build()
        {
            mSpriteAtlasDict.Clear();
            mAssetItemDict.Clear();
            CreateSpriteAtlasMap();
            CreateAssetDependsMap();
            GroupAssetBundles();
            CreateAssetBundleConfig();
            SetAssetBundleNames();
            BuildAssetBundles();
            ClearAssetBundleNames();
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 根据平台构建AssetBundle
        /// </summary>
        public static void Build(BuildTarget buildTarget)
        {
            mBuildTarget = buildTarget;
            Build();
        }

        /// <summary>
        /// 遍历Atlas下所有的SpriteAtlas，建立图片资源依赖
        /// </summary>
        public static void CreateSpriteAtlasMap()
        {
            var dir = new DirectoryInfo(BuilderConfig.SpriteAtlasPath);
            if (!dir.Exists) return;
            var files = dir.GetFiles();
            for (var i = 0; i < files.Length; i++)
            {
                var fileInfo = files[i];
                EditorUtility.DisplayProgressBar("CreateSpriteAtlasMap", fileInfo.FullName, 1.0f * (i + 1) / files.Length);
                if (AssetUtils.ValidAsset(fileInfo.FullName))
                {
                    var fullPath = fileInfo.FullName.Replace("\\", "/");
                    var path = fullPath.Substring(fullPath.IndexOf(BuilderConfig.AssetRootPath));
                    var assetItem = GetAssetItem(path);
                    var depends = AssetDatabase.GetDependencies(path);
                    foreach (var depend in depends)
                    {
                        if (AssetUtils.ValidAsset(depend) && depend != path)
                        {
                            mSpriteAtlasDict.Add(depend, path);
                            assetItem.Depends.Add(depend);
                            var dependAssetItem = GetAssetItem(depend);
                            dependAssetItem.BeDepends.Add(path);
                        }
                    }
                    mSpriteAtlasDict.Add(path, path);
                }
            }
        }

        /// <summary>
        /// 遍历资源，建立依赖映射
        /// </summary>
        public static void CreateAssetDependsMap()
        {
            var stack = new Stack<DirectoryInfo>();
            stack.Push(new DirectoryInfo(BuilderConfig.AssetRootPath));
            while (stack.Count > 0)
            {
                var dirInfo = stack.Pop();
                var childDirs = dirInfo.GetDirectories();
                if (childDirs != null)
                {
                    foreach (var dir in childDirs)
                    {
                        stack.Push(dir);
                    }
                }
                var files = dirInfo.GetFiles();
                for (var i = 0; i < files.Length; i++)
                {
                    var fileInfo = files[i];
                    EditorUtility.DisplayProgressBar("CreateAssetDependsMap", fileInfo.FullName, 1.0f * (i + 1) / files.Length);
                    if (AssetUtils.ValidAsset(fileInfo.FullName))
                    {
                        // 处理依赖相关
                        var fullPath = fileInfo.FullName.Replace("\\", "/");
                        var path = fullPath.Substring(fullPath.IndexOf(BuilderConfig.AssetRootPath));
                        // 过滤掉在图集中的资源
                        if (!mSpriteAtlasDict.ContainsKey(path))
                        {
                            var assetItem = GetAssetItem(path);
                            var depends = AssetDatabase.GetDependencies(path);
                            foreach (var depend in depends)
                            {
                                if (AssetUtils.ValidAsset(depend) && depend != path)
                                {
                                    // 如果依赖的Sprite有对应的图集，就改为依赖SpriteAtlas
                                    var dependPath = mSpriteAtlasDict.ContainsKey(depend) ? mSpriteAtlasDict[depend] : depend;
                                    if (!assetItem.Depends.Contains(dependPath))
                                    {
                                        assetItem.Depends.Add(dependPath);
                                        var dependAssetItem = GetAssetItem(dependPath);
                                        dependAssetItem.BeDepends.Add(path);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获得资源信息
        /// </summary>
        private static AssetItem GetAssetItem(string path)
        {
            AssetItem item;
            if (!mAssetItemDict.TryGetValue(path, out item))
            {
                item = new AssetItem();
                item.AssetBundleName = GetAssetBundleName(path);
                mAssetItemDict.Add(path, item);
            }
            return item;
        }

        /// <summary>
        /// 合并依赖，并分组资源
        /// </summary>
        public static void GroupAssetBundles()
        {
            /* 清除同层依赖 把同层之间被依赖的节点下移 (a->b, a->c, b->c) ==> (a->b->c)
             *      a              a
             *     /  \    ==>    /
             *    b -> c         b
             *                  /
             *                 c 
             *  例如：prefab上挂着mat, mat依赖shder。特别注意，此时prefab同时依赖mat,和shader。可以点击右键查看
             *  (prefab->mat, prefab->shader, mat->shader) ==> (prefab->mat->shader)
             */
            var removeList = new List<string>();
            foreach (var item in mAssetItemDict)
            {
                removeList.Clear();
                var path = item.Key;
                var assetItem = item.Value;
                foreach (var depend in assetItem.Depends)
                {
                    var dependAssetItem = GetAssetItem(depend);
                    foreach (var beDepend in dependAssetItem.BeDepends)
                    {
                        if (assetItem.Depends.Contains(beDepend))
                            removeList.Add(depend);
                    }
                }
                foreach (var depend in removeList)
                {
                    assetItem.Depends.Remove(depend);
                    var dependAssetItem = GetAssetItem(depend);
                    dependAssetItem.BeDepends.Remove(path);
                }
            }

            /* 向上归并依赖
             *      a        e                 
             *       \      /                    
             *        b    f     ==>  (a,b,c,h) -> (d) <- (e,f)
             *      / | \ /                          
             *     c  h  d      
             */
            foreach (var item in mAssetItemDict)
            {
                var path = item.Key;
                var assetItem = item.Value;
                while (assetItem.BeDepends.Count == 1)
                {
                    assetItem = GetAssetItem(assetItem.BeDepends[0]);
                    if (assetItem.BeDepends.Count != 1)
                    {
                        item.Value.AssetBundleName = assetItem.AssetBundleName;
                    }
                }
            }
        }

        /// <summary>
        /// 生成AssetBundleConfig配置文件
        /// </summary>
        private static void CreateAssetBundleConfig()
        {
            var configPath = BuilderConfig.PathBundleConfig;
            var config = new PathBundleInfoList();
            config.List.Add(new PathBundleInfo() { Path = configPath, AssetBundleName = configPath });
            foreach (var item in mAssetItemDict)
            {
                if (item.Key != configPath)
                {
                    var pathBundleInfo = new PathBundleInfo();
                    pathBundleInfo.Path = item.Key;
                    pathBundleInfo.AssetBundleName = item.Value.AssetBundleName;
                    config.List.Add(pathBundleInfo);
                }
            }
            var buffer = Base.Utils.ProtobufUtil.NSerialize(config);
            Base.Utils.FileUtil.WriteAllBytes(configPath, buffer);
            var assetItem = GetAssetItem(configPath);
            assetItem.AssetBundleName = GetAssetBundleName(configPath);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 设置所有的ABName
        /// </summary>
        private static void SetAssetBundleNames()
        {
            foreach (var item in mAssetItemDict)
            {
                var path = item.Key;
                var assetItem = item.Value;
                var assetImport = AssetImporter.GetAtPath(path);
                if (assetImport != null)
                {
                    assetImport.assetBundleName = assetItem.AssetBundleName.ToLower();
                }
            }
        }

        /// <summary>
        /// 获取完整AssetBundle包名
        /// </summary>
        private static string GetAssetBundleName(string path)
        {
            return (path + ASSETBUNDLEEX).ToLower();
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
        /// 生成AssetBundles
        /// </summary>
        private static void BuildAssetBundles()
        {
            if (mBuildTarget == BuildTarget.NoTarget)
            {
#if UNITY_EDITOR_OSX
                mBuildTarget = BuildTarget.StandaloneOSX;
#elif UNITY_EDITOR_WIN
                mBuildTarget = BuildTarget.StandaloneWindows;
#endif              
            }
            EditorUtility.DisplayProgressBar("BuildAssetBundles", "", 0);
            if (Directory.Exists(BuilderConfig.AssetBundleExportPath))
            {
                FileUtil.DeleteFileOrDirectory(BuilderConfig.AssetBundleExportPath);
            }
            Directory.CreateDirectory(BuilderConfig.AssetBundleExportPath);
            BuildPipeline.BuildAssetBundles(BuilderConfig.AssetBundleExportPath, BuilderConfig.Options, mBuildTarget);
            mBuildTarget = BuildTarget.NoTarget;
            Debugger.Log("AssetBundle Build Success!");
        }
    }
}
