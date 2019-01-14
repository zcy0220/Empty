/**
 * 程序主入口
 */

using UnityEngine;

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
