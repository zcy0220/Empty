/**
 * Lua管理
 * 1.编辑模式下：Assets/LuaScripts
 * 2.AssetBundle：*.lua -> *.lua.bytes 并移到Assets/GameAssets/LuaScripts目录下进行AB打包
 */

using XLua;
using System;
using Base.Common;
using Base.Debug;
using Base.Utils;
using UnityEngine;
using Base.Extension;

public class LuaManager : MonoSingleton<LuaManager>
{
    /// <summary>
    /// lua脚本路径
    /// </summary>
    const string LUASCRIPTFOLDER = "LuaScripts";
    /// <summary>
    /// Lua脚本入口
    /// </summary>
    const string LUAMAINSCRIPT = "GameMain";
    /// <summary>
    /// lua环境
    /// </summary>
    private LuaEnv mLuaEnv;
    /// <summary>
    /// Lua暂时先只用Update
    /// </summary>
    private Action<float> mLuaUpdate;
    
    /// <summary>
    /// 初始化Lua虚拟器环境
    /// </summary>
    private void InitLuaEnv()
    {
        mLuaEnv = new LuaEnv();
        mLuaEnv.AddLoader(CustomLoader);
    }

    /// <summary>
    /// 重启
    /// </summary>
    public void Restart()
    {
        Dispose();
        InitLuaEnv();
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public override void Dispose()
    {
        if (mLuaEnv != null)
        {
            ExecuteScript("GameMain.Dispose()");
            mLuaEnv.Dispose();
            mLuaEnv = null;
        }
    }
    
    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        LoadScript(LUAMAINSCRIPT);
        ExecuteScript("GameMain.Start()");
        var updater = gameObject.GetOrAddComponent<LuaUpdater>();
        updater.Init(mLuaEnv);
    }

    /// <summary>
    /// 加载脚本
    /// </summary>
    private void LoadScript(string scriptName)
    {
        ExecuteScript(string.Format("require('{0}')", scriptName));
    }
    
    /// <summary>
    /// 执行脚本
    /// </summary>
    private void ExecuteScript(string script)
    {
        if (mLuaEnv != null)
        {
            try
            {
                mLuaEnv.DoString(script);
            }
            catch (System.Exception e)
            {
                Debugger.LogError(e.Message);
            }
        }
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        if (mLuaUpdate != null) mLuaUpdate(Time.deltaTime);
        if (mLuaEnv != null) mLuaEnv.Tick();
    }

    /// <summary>
    /// 自定义Lua加载器
    /// 真机上的lua脚本加后缀.bytes
    /// </summary>
    private byte[] CustomLoader(ref string filepath)
    {
        filepath = filepath.Replace(".", "/") + ".lua";
        var scriptPath = StringUtil.PathConcat(LUASCRIPTFOLDER, filepath);
        if (AppConfig.UseAssetBundle)
        {
            scriptPath = StringUtil.Concat(scriptPath, ".bytes");
            var asset = ResourceManager.Instance.SyncLoad<TextAsset>(scriptPath);
            if (asset != null) return asset.bytes;
            Debugger.LogError("Load lua script failed: " + scriptPath);
            return null;
        }
        else
        {
            return FileUtil.ReadAllBytes(StringUtil.PathConcat(Application.dataPath, scriptPath));
        }
    }
}
