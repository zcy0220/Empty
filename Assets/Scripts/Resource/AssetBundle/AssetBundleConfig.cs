/**
 * AssetBundle配置
 */

using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

public class AssetBundleConfig
{
    public const string AssetBundlesFolder = "AssetBundles";
    public static readonly string AssetBundlesPath = Application.streamingAssetsPath + "/" + AssetBundlesFolder;
    public static readonly string PathBundleConfig = "Assets/GameAssets/Config/PathBundleConfig.bytes";
}

[ProtoContract]
public class PathBundleInfoList
{
    [ProtoMember(1)]
    public List<PathBundleInfo> List = new List<PathBundleInfo>();
}

[ProtoContract]
public class PathBundleInfo
{
    [ProtoMember(1)]
    public string Path;
    [ProtoMember(2)]
    public string AssetBundleName;
}
