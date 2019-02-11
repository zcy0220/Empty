/**
 * 资源单项
 */

using UnityEngine;
using System.Collections.Generic;

public class ResourceItem
{
    public string MD5;
    public string AssetName;
    public string ABName;
    public List<string> ABDependList;
    public AssetBundle AssetBundle;
    //===============================
    /// <summary>
    /// 资源唯一标志
    /// </summary>
    public int Guid;
    /// <summary>
    /// 资源对象
    /// </summary>
    public Object Obj;
    /// <summary>
    /// 资源最后使用的时间
    /// </summary>
    public float LastUseTime;
    /// <summary>
    /// 引用次数
    /// </summary>
    public int RefCount;
}
