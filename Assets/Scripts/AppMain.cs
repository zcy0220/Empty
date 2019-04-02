/**
 * 程序主入口
 */

using Base.Common;

public class AppMain : MonoSingleton<AppMain>
{
    /// <summary>
    /// 启动
    /// </summary>
    public void Startup()
    {
        // 测试表格数据
        SheetManager.Instance.Test();
    }
}
