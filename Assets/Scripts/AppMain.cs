/**
 * 程序主入口
 */

using Sheet;
using ProtoBuf;
using UnityEngine;
using System.IO;
using Base.Utils;

public class AppMain : MonoBehaviour
{
    /// <summary>
    /// 开始游戏，初始化配置文件
    /// </summary>
    private void Awake()
    {
        SheetTest.Test();
    }
}
