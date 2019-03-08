/**
 * 场景管理
 */

using Base.Common;
using Base.Extension;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoSingleton<ScenesManager>
{
    /// <summary>
    /// 异步加载
    /// </summary>
    public void LoadSceneAsync(string name)
    {
        var loader = gameObject.GetOrAddComponent<SceneLoader>();
        loader.Init(name);
        loader.StartLoad();
    }

    /// <summary>
    /// 同步加载
    /// </summary>
    public void LoadSceneSync(string name)
    {
        SceneManager.LoadScene(name);
    }
}
