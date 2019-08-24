/**
 * 场景控制器
 */

using UnityEngine;
using Base.Common;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoSingleton<SceneController>
{
    /// <summary>
    /// 当前场景
    /// </summary>
    private BaseScene mCurScene;
    /// <summary>
    /// 进度条
    /// </summary>
    private float mLoadProgress;
    /// <summary>
    /// 加载场景
    /// </summary>
    private const string LOADING_SCENE = "LoadingScene";

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="immediately">直接进入 主要用于StartScene</param>
    public void LoadScene(BaseScene scene, bool immediately = false)
    {
        if (mCurScene != null) mCurScene.Exit();

        mCurScene = scene;

        if (immediately)
        {
            mCurScene.Enter();
        }
        else
        {
            StartCoroutine(StartLoad());
        }
    }

    /// <summary>
    /// 开始加载
    /// </summary>
    IEnumerator StartLoad()
    {
        //打开Loading界面，隔帧开始下一步
        ViewManager.Instance.Add<LoadingView>(ViewSiblingIndex.HIGH);
        yield return null;
        //同步加载空场景
        SceneManager.LoadScene(LOADING_SCENE);
        yield return null;
        // 卸载没用的资源
        Resources.UnloadUnusedAssets();
        // GC处理
        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();
        yield return null;
        // 场景预加载
        mCurScene.Preload();
        while(!mCurScene.IsPreloadDone)
        {
            yield return null;
        }
        yield return null;
        // 异步加载对应场景
        var asyncLoad = SceneManager.LoadSceneAsync(mCurScene.SceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        // 加载结束
        mCurScene.Enter();
        ViewManager.Instance.Close<LoadingView>();
    }

    /// <summary>
    /// 实时更新
    /// </summary>
    public void Update()
    {
        if (mCurScene != null) mCurScene.Update();
    }
}
