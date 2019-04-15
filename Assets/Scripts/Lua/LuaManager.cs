/**
 * Lua管理
 */

using XLua;
using Base.Common;

public class LuaManager : MonoSingleton<LuaManager>
{

    const string LUAMAINSCRIPT = "Main";
    /// <summary>
    /// lua环境
    /// </summary>
    private LuaEnv mLuaEnv;
}
