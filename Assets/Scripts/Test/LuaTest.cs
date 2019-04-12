/**
 * lua测试
 */

using XLua;
using UnityEngine;

public class LuaTest : MonoBehaviour
{
	void Start()
	{
        var luaenv = new LuaEnv();
        luaenv.DoString("CS.UnityEngine.Debug.Log('Lua Hello World!')");
        luaenv.Dispose();
	}
}
