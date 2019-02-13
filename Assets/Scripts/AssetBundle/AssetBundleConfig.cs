/**
 * AssetBundle配置
 */

using System.Xml.Serialization;
using System.Collections.Generic;

[System.Serializable]
public class AssetBundleConfig
{
    [XmlElement("AssetBundleList")]
    public List<AssetBundleBase> AssetBundleList;
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
