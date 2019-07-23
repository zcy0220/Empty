# 场景管理
* 状态模式管理场景的切换

## 场景切换流程
~~~C#
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
~~~

## 预加载
* 这里用一个表格来简单演示预加载的内容
* 对象池缓存预加载的对象
~~~C#
/// <summary>
/// BaseScene基类中的方法
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
~~~