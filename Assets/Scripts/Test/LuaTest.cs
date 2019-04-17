/**
 * lua测试
 */

using XLua;
using System;
using UnityEngine;

//[LuaCallCSharp]
public class LuaTest : MonoBehaviour
{
    public TextAsset LuaScript;

    string script = @"
        function sum(a, b)
            print('a', a, 'b', b)
            return a + b
        end
    ";

    //[CSharpCallLua]
    public delegate void test(int a, int b);

	private void Awake()
	{
        LuaEnv luaEnv = new LuaEnv();
        luaEnv.DoString(script);
        //LuaTable scriptEnv = luaEnv.NewTable();
        //luaEnv.DoString(LuaScript.text, "LuaTest", scriptEnv);
        //LuaTable meta = luaEnv.NewTable();
        //meta.Set("__index", luaEnv.Global);
        //scriptEnv.SetMetaTable(meta);
        //meta.Dispose();
        //var sum = scriptEnv.Get<test>("sum");
        //sum(1, 2);
        var sum = luaEnv.Global.Get<test>("sum");
        sum(1, 2);
        //luaEnv.Dispose();
    }
}
