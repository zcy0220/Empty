/**
 * AssetBundle配置
 */

using System.Xml.Serialization;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

public class AssetBundleConfig
{
    public static readonly string AssetBundlesPath = Application.streamingAssetsPath + "/AssetBundles";
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


[System.Serializable]
public class AssetBundleBase
{
    [XmlAttribute("Path")]
    public string Path;
    [XmlAttribute("ABName")]
    public string ABName;
    [XmlAttribute("AssetName")]
    public string AssetName;
    [XmlAttribute("ABDependList")]
    public List<string> ABDependList;
}
