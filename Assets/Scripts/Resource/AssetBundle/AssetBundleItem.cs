/**
 * AssetBundleItem
 */

using UnityEngine;

public class AssetBundleItem
{
    /// <summary>
    /// 对应的AssetBundle引用
    /// </summary>
    public AssetBundle AssetBundle;
    /// <summary>
    /// 引用计数
    /// </summary>
    public int RefCount;

    /// <summary>
    /// 重置方法
    /// </summary>
    public void Reset()
    {
        AssetBundle = null;
        RefCount = 0;
    }
}
