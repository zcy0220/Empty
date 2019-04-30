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
        LuaManager.Instance.Restart();
        LuaManager.Instance.StartGame();
    }
}
