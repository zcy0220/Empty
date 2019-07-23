/**
 * 视图配置表
 */

using System;
using System.Collections.Generic;

public class ViewConfig
{
    /// <summary>
    /// 类型和Prefab路径的对应
    /// </summary>
    private static Dictionary<Type, string> mTypePathDict;

    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init()
    {
        mTypePathDict = new Dictionary<Type, string>();
        mTypePathDict.Add(typeof(MainView), "Prefabs/Views/Main/MainView.prefab");
        mTypePathDict.Add(typeof(BattleView), "Prefabs/Views/Battle/BattleView.prefab");
        mTypePathDict.Add(typeof(LoadingView), "Prefabs/Views/Common/LoadingView.prefab");
    }

    /// <summary>
    /// 获得UI路径
    /// </summary>
    public static string GetViewPath(Type type)
    {
        string path = string.Empty;
        mTypePathDict.TryGetValue(type, out path);
        return path;
    }
}
