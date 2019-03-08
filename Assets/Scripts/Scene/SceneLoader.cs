/**
 * 场景加载器
 */

using System;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// 场景名
    /// </summary>
    private string mName;

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(string name)
    {
        mName = name;
    }

    /// <summary>
    /// 开始异步加载
    /// </summary>
    public void StartLoad()
    {
    }
}
