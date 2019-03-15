/**
 * 主界面场景
 */

public class MainScene : BaseScene
{
    public override void Enter()
    {
        base.Enter();
        AppMain.Instance.Startup();
    }
}
