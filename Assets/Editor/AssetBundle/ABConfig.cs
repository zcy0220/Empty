/**
 * AssetBundle手动打包策略配置
 * 手动配置好要打包的prefab和文件夹
 */

using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ABConfig", menuName = "CreateABConfig", order = 10000)]
public class ABConfig : SerializedScriptableObject
{
    /// <summary>
    /// 要打包文件夹下的所有prefab的路径
    /// </summary>
    public List<string> PrefabDirPathList;
    /// <summary>
    /// AB包名和对应的文件夹路径
    /// </summary>
    [DictionaryDrawerSettings(KeyLabel = "AssetBundleName", ValueLabel = "DirectoryPath")]
    public Dictionary<string, string> DirectoryPathDict;
}
