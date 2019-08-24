/**
 * 场景基类
 */

public abstract class BaseScene
{
    /// <summary>
    /// 场景表格数据
    /// </summary>
    protected Sheet.Scene mSceneSheet;
    /// <summary>
    /// 场景Id
    /// </summary>
    public int SceneId { get; protected set; }
    /// <summary>
    /// 场景名
    /// </summary>
    public string SceneName { get; protected set; }
    /// <summary>
    /// 是否预加载完成
    /// </summary>
    public bool IsPreloadDone { get; protected set; }

    /// <summary>
    /// 无参构造
    /// </summary>
    public BaseScene() { }

    /// <summary>
    /// 场景Id构造
    /// </summary>
    public BaseScene(int sceneId) { SceneId = sceneId; }

    /// <summary>
    /// 根据名字和Id构造
    /// </summary>
    public BaseScene(int sceneId, string sceneName) { SceneId = sceneId; SceneName = sceneName; }

    /// <summary>
    /// 场景进入
    /// </summary>
    public virtual void Enter() { }

    /// <summary>
    /// 场景退出
    /// </summary>
    public virtual void Exit() { }

    /// <summary>
    /// 场景更新
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// 预加载资源
    /// </summary>
    public virtual void Preload()
    {
        IsPreloadDone = true;
        if (SceneId != 0)
        {
            var list = SheetManager.Instance.GetPreloadSheet(SceneId);
            if (list != null && list.Count > 0)
            {
                IsPreloadDone = false;
                var count = 0;
                foreach (var item in list)
                {
                    GameObjectManager.Instance.PreLoadGameObject(item.ResPath, () =>
                    {
                        count++;
                        if (count == list.Count) IsPreloadDone = true;
                    });
                }
            }
        }
    }

    /// <summary>
    /// 初始化场景表格数据
    /// </summary>
    protected virtual void InitSceneSheet()
    {
        if (SceneId == 0) return;
        mSceneSheet = SheetManager.Instance.GetScene(SceneId);
        if (string.IsNullOrEmpty(SceneName)) SceneName = mSceneSheet.Name;
    }
}
