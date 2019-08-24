/**
 * 战斗场景
 */

public class BattleScene : BaseScene
{
    /// <summary>
    /// 构造
    /// </summary>
    public BattleScene(int sceneId) : base(sceneId)
    {
        InitSceneSheet();
    }

    /// <summary>
    /// 进入战斗
    /// </summary>
    public override void Enter()
    {
        ViewManager.Instance.Open<BattleView>();
    }

    /// <summary>
    /// 退出战斗
    /// </summary>
    public override void Exit()
    {
        if (SceneId != 0)
        {
            var list = SheetManager.Instance.GetPreloadSheet(SceneId);
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    GameObjectManager.Instance.DisposeGameObjectPool(item.ResPath);
                }
            }
        }
    }

    /// <summary>
    /// 检测
    /// </summary>
    public override void Update()
    {
    }
}
