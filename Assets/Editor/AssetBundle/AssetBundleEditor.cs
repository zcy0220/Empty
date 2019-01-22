/**
 * AssetBundle打包编辑
 */

using System;
using UnityEditor;
using UnityEngine;

public class AssetBundleEditor
{
    [MenuItem("Tools/AssetBundle/Build")]
    public static void Build()
    {
        //BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, 0, BuildTarget.ac)
    }
}
