/**
 * AssetBundle手动打包策略配置
 * 手动配置好要打包的prefab和文件夹
 */

using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ABManualConfig", menuName = "CreateABManualConfig", order = 10000)]
public class ABManualConfig : SerializedScriptableObject
{
    /// <summary>
    /// 所有要打包的prefab路径
    /// </summary>
    public List<string> PrefabPathList;
    /// <summary>
    /// 文件夹路径及其对应包名
    /// </summary>
    [DictionaryDrawerSettings(KeyLabel = "DirectoryPath", ValueLabel = "AssetBundleName")]
    public Dictionary<string, string> DirectoryPathDict;
}
