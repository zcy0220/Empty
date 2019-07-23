/**
 * 开始场景
 */

public class StartScene : BaseScene
{
    /// <summary>
    /// 构造
    /// </summary>
    public StartScene()
    {
        SceneName = "StartScene";
    }

    /// <summary>
    /// 开始场景进入
    /// 先检测热更新
    /// </summary>
    public override void Enter()
    {
        if (AppConfig.CheckVersionUpdate)
        {
            ResourceUpdater.Instance.Startup();
            ResourceUpdater.Instance.OnFinished = OnFinished;
        }
        else
        {
            Preload();
        }
    }
    
    /// <summary>
    /// 热更新资源检测下载结束 
    /// </summary>
    private void OnFinished()
    {
        ResourceUpdater.Instance.Destroy();
        Preload();
    }

    /// <summary>
    /// 预加载资源
    /// </summary>
    public override void Preload()
    {
        IsPreloadDone = true;
        OnPreloadFinished();
    }

    /// <summary>
    /// 预加载资源结束，跳转主界面场景
    /// </summary>
    private void OnPreloadFinished()
    {
        SceneController.Instance.LoadScene(new MainScene());
    }
}
