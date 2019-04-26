/**
 * xlua脚本导出辅助类
 */

using XLua;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
public static class LuaExporter
{
    /// <summary>
    /// 类为主
    /// </summary>
    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>()
    {
    };

    /// <summary>
    /// 委托为主
    /// </summary>
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>()
    {
        typeof(Action),
        typeof(Action<float>),
        typeof(Action<float, float>)
    };
}
#endif
