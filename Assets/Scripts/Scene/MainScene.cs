/**
 * 主界面场景
 */

public class MainScene : BaseScene
{
    /// <summary>
    /// 主场景Id
    /// </summary>
    private const int MAIN_SCENE_ID = 100;

    /// <summary>
    /// 构造
    /// </summary>
    public MainScene()
    {
        SceneId = MAIN_SCENE_ID;
        SceneName = "MainScene";
        InitSceneSheet();
    }

    /// <summary>
    /// 主场景进入
    /// </summary>
    public override void Enter()
    {
        ViewManager.Instance.Open<MainView>();
    }
}
