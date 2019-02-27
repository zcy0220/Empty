/**
 * AssetBundle打包构建
 */

using System.IO;
using UnityEditor;
using System.Collections.Generic;

namespace Assets.Editor.AssetBundle
{
    public class AssetBundleBuilder
    {
        /// <summary>
        /// 资源依赖映射
        /// </summary>
        private static Dictionary<string, AssetItem> mAssetItemDict = new Dictionary<string, AssetItem>();

        [MenuItem("Tools/AssetBundle/Build")]
        public static void Build()
        {
            CreateAssetDependsMap();
            GroupAssetBundles();
        }

        /// <summary>
        /// 遍历资源，建立依赖映射
        /// </summary>
        public static void CreateAssetDependsMap()
        {
            mAssetItemDict.Clear();
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
                foreach(var fileInfo in files)
                {
                    if (AssetUtils.ValidAsset(fileInfo.FullName))
                    {
                        // 处理依赖相关
                        var fullPath = fileInfo.FullName.Replace("\\", "/");
                        var path = fullPath.Substring(fullPath.IndexOf(BuilderConfig.AssetRootPath));
                        var assetItem = GetAssetItem(path);
                        var depends = AssetDatabase.GetDependencies(path);
                        foreach(var depend in depends)
                        {
                            if (AssetUtils.ValidAsset(depend) && depend != path)
                            {
                                assetItem.Depends.Add(depend);
                                var dependAssetItem = GetAssetItem(depend);
                                dependAssetItem.BeDepends.Add(path);
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
             *  例如：prefab上挂着mat, mat依赖shder。(prefab->mat, prefab->shader, mat->shader) ==> (prefab->mat->shader)
             */
            var removeList = new List<string>();
            foreach(var item in mAssetItemDict)
            {
                removeList.Clear();
                var path = item.Key;
                var assetItem = item.Value;
                foreach(var depend in assetItem.Depends)
                {
                    var dependAssetItem = GetAssetItem(depend);
                    foreach(var beDepend in dependAssetItem.BeDepends)
                    {
                        if (assetItem.Depends.Contains(beDepend))
                            removeList.Add(depend);
                    }
                }
                foreach(var depend in removeList)
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
        }


        /// <summary>
        /// 生成AssetBundles
        /// </summary>
        private static void BuildAssetBundles()
        {
            if (!Directory.Exists(BuilderConfig.AssetBundleExportPath))
            {
                Directory.CreateDirectory(BuilderConfig.AssetBundleExportPath);
            }
            BuildPipeline.BuildAssetBundles(BuilderConfig.AssetBundleExportPath, BuilderConfig.Options, BuilderConfig.Target);
        }
    }
}
