/**
 * Lua更新
 */

using XLua;
using System;
using UnityEngine;
 
public class LuaUpdater : MonoBehaviour
{
    private LuaEnv mLuaEnv;
    private Action<float, float> mUpdate;
    private Action mLateUpdate;
    private Action<float> mFixedUpdate;

    /// <summary>
    /// 绑定Lua环境
    /// </summary>
    /// <param name="luaEnv"></param>
    public void Init(LuaEnv luaEnv)
    {
        mLuaEnv = luaEnv;
        mUpdate = mLuaEnv.Global.Get<Action<float, float>>("Update");
        mLateUpdate = mLuaEnv.Global.Get<Action>("LateUpdate");
        mFixedUpdate = mLuaEnv.Global.Get<Action<float>>("FixedUpdate");
    }

    /// <summary>
    /// Update 把deltaTime、unscaledDeltaTime都传给Lua层
    /// </summary>
    private void Update()
    {
        if (mUpdate != null) mUpdate(Time.deltaTime, Time.unscaledDeltaTime);
    }

    /// <summary>
    /// LateUpdate
    /// </summary>
    private void LateUpdate()
    {
        if (mLateUpdate != null) mLateUpdate();
    }
    
    /// <summary>
    /// FixedUpdate
    /// </summary>
    private void FixedUpdate()
    {
        if (mFixedUpdate != null) mFixedUpdate(Time.fixedDeltaTime);
    }

    /// <summary>
    /// 销毁
    /// </summary>
    private void OnDestroy()
    {
        mUpdate = null;
        mLateUpdate = null;
        mFixedUpdate = null;
    }
}
