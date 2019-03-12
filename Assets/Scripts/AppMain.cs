/**
 * 程序主入口
 */

using UnityEngine;

public class AppMain : MonoBehaviour
{
    /// <summary>
    /// 开始游戏，初始化配置文件
    /// </summary>
    private void Awake()
    {
        AppConfig.UseAssetBundle = true;
        //var prefab1 = ResourceManager.Instance.SyncLoad<GameObject>("Assets/GameAssets/Prefabs/ExamplePrefab1.prefab");
        //GameObject.Instantiate(prefab1);
        //var prefab2 = ResourceManager.Instance.SyncLoad<GameObject>("Assets/GameAssets/Prefabs/ExamplePrefab2.prefab");
        //GameObject.Instantiate(prefab2);
        //ResourceManager.Instance.AsyncLoad<GameObject>("Assets/GameAssets/Prefabs/ExamplePrefab1.prefab", (obj) =>
        //{
        //    GameObject.Instantiate(obj);
        //});
        //ResourceManager.Instance.AsyncLoad<GameObject>("Assets/GameAssets/Prefabs/ExamplePrefab2.prefab", (obj) =>
        //{
        //    GameObject.Instantiate(obj);
        //});
        SheetManager.Instance.Test();
    }
}
