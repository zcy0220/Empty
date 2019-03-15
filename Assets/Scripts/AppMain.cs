/**
 * 程序主入口
 */

using Base.Common;
using UnityEngine;

public class AppMain : MonoSingleton<AppMain>
{
    /// <summary>
    /// 启动
    /// </summary>
    public void Startup()
    {
        // 测试同步加载
        var prefab1 = ResourceManager.Instance.SyncLoad<GameObject>("Assets/GameAssets/Prefabs/ExamplePrefab1.prefab");
        GameObject.Instantiate(prefab1);
        // 测试异步加载
        ResourceManager.Instance.AsyncLoad<GameObject>("Assets/GameAssets/Prefabs/ExamplePrefab2.prefab", (obj) =>
        {
            GameObject.Instantiate(obj);
        });
        // 测试表格数据
        SheetManager.Instance.Test();
    }
}
