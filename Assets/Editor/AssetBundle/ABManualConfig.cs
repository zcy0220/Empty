/**
 * AssetBundle手动打包策略配置
 * 手动配置好要打包的prefab和文件夹
 */

using Sirenix.OdinInspector;
using System.Collections.Generic;

public class ABManualConfig : SerializedScriptableObject
{
    /// <summary>
    /// 所有要打包的prefab路径
    /// </summary>
    public List<string> mPrefabPathList;
}
