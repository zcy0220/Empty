/**
 * 全局方法
 * 逻辑相关
 */

using Base.Utils;

public class GlobalAPI
{
    /// <summary>
    /// 获取模型路径
    /// </summary>
    /// <param name="path">相对 Prefabs/Models 下的路径</param>
    /// <returns></returns>
    public static string GetEntityModelPath(string path)
    {
        return StringUtil.PathConcat("Prefabs/Models/Entitys", StringUtil.Concat(path, ".prefab"));
    }
}
