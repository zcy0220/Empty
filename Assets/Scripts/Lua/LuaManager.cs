/**
 * Lua管理
 * 1.编辑模式下：Assets/LuaScripts
 * 2.AssetBundle：*.lua -> *.lua.bytes 并移到Assets/GameAssets/LuaScripts目录下进行AB打包
 */

using XLua;
using Base.Common;
using Base.Debug;
using Base.Utils;
using UnityEngine;

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
