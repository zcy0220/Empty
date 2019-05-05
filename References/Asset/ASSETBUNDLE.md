# AssetBundle打包

## 一键打包
* Tools->AssetBundle->Build

## 打包策略
* 冗余情况：A依赖C，B依赖C，如果只打成A,B两个Bundle包，就会冗余C。每个资源文件单独打成一个Bundle包，简单方便，同时保证不冗余，但会造成Bundle数目过多
* 粒度问题：A依赖B、C、D，按单资源打成单Bundle包的策略会打成A，B，C，D四个Bundle，加载依赖是要加载4个包，但如果C，D是被A所唯一依赖的，那么更好的策略就是A，C，D打成一个Bundle包，那么加载依赖就只用加载2个Bundle包，提高了加载效率，同时也不会冗余资源。（类似散图和图集的关系，但具体两者差距多少，并没有实际测过）
* 打包策略：保证不冗余的情况下，最大限度的减少粒度
    - 指定根目录，记录所有文件和其被依赖列表
    - 文件被依赖数大于1，就单独打成Bundle包，否则向上归并至被依赖项

## 核心代码
~~~C#
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
        foreach(var item in mAssetItemDict)
        {
            var path = item.Key;
            var assetItem = item.Value;
            while(assetItem.BeDepends.Count == 1)
            {
                assetItem = GetAssetItem(assetItem.BeDepends[0]);
                if (assetItem.BeDepends.Count != 1)
                {
                    item.Value.AssetBundleName = assetItem.AssetBundleName;
                }
            }
        }
    }
~~~

## 追加图集的打包策略
* SpriteAtlas和其关联的所有资源打成一个AssetBundle包
* 建立AssetBundle依赖映射时，如果依赖的Sprite有对应的图集，就改为依赖SpriteAtlas
~~~C#
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
~~~

## 追加Lua的打包策略（在xlua分支上）
* Assets/LuaScripts/*.lua -> Assets/GameAssets/LuaScripts/*.lua.bytes
~~~C#
/// <summary>
/// 创建Lua二进制文件
/// Assets/LuaScripts/*.lua -> Assets/GameAssets/LuaScripts/*.lua.bytes
/// </summary>
public static void CreateLuaBytes()
{
    if (Directory.Exists(BuilderConfig.LuaScriptsDestPath))
    {
        FileUtil.DeleteFileOrDirectory(BuilderConfig.LuaScriptsDestPath);
    }
    var dir = new DirectoryInfo(BuilderConfig.LuaScriptsSrcPath);
    if (!dir.Exists) return;
    var stack = new Stack<DirectoryInfo>();
    stack.Push(dir);
    while (stack.Count > 0)
    {
        var dirInfo = stack.Pop();
        var files = dirInfo.GetFiles();

        foreach (var file in files)
        {
            if (AssetUtils.ValidAsset(file.FullName))
            {
                var fullPath = file.FullName.Replace("\\", "/");
                var startIndex = fullPath.IndexOf("LuaScripts");
                var destPath = fullPath.Insert(startIndex, "GameAssets/") + ".bytes";
                if (Base.Utils.FileUtil.CheckFileAndCreateDirWhenNeeded(destPath))
                {
                    file.CopyTo(destPath);
                }
            }
        }

        var subDirs = dirInfo.GetDirectories();
        foreach (var subDir in subDirs)
        {
            stack.Push(subDir);
        }
    }
}
~~~