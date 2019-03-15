/**
 * 场景管理
 */

using Base.Common;
using Base.Debug;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoSingleton<ScenesManager>
{
    /// <summary>
    /// 当前场景名
    /// </summary>
    private string mCurSceneName;
    /// <summary>
    /// 当前场景
    /// </summary>
    private BaseScene mCurScene;

    /// <summary>
    /// 加载场景
    /// </summary>
    public void LoadScene(string name)
    {
        Debugger.Log("Start load {0}", mCurSceneName);
        mCurSceneName = name;
        // todo loading界面
        if (mCurScene != null) mCurScene.Exit();
        // todo 场景使用AssetBundle加载的，那么要先加载对应的AssetBundle
        mCurScene = GetScene(mCurSceneName);
        StartCoroutine(StartLoad());
    }
    
    /// <summary>
    /// 根据场景名创建对应场景
    /// </summary>
    private BaseScene GetScene(string sceneName)
    {
        switch(sceneName)
        {
            default:
                return new MainScene();
        }
    }
    
    /// <summary>
    /// 开始加载
    /// </summary>
    IEnumerator StartLoad()
    {
        // todo 加载进度
        // todo 预加载
        var asyncLoad = SceneManager.LoadSceneAsync(mCurSceneName);
        while(!asyncLoad.isDone)
        {
            yield return null;
        }
        FinishLoad();        
    }
    
    /// <summary>
    /// 加载完成
    /// </summary>
    private void FinishLoad()
    {
        mCurScene.Enter();
    }
}
