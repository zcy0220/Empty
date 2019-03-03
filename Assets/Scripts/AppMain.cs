/**
 * 程序主入口
 */

using UnityEngine;

public class AppMain : MonoBehaviour
{
    private GameObject mExampleGo;

    /// <summary>
    /// 开始游戏，初始化配置文件
    /// </summary>
    private void Awake()
    {
        AppConfig.UseAssetBundle = true;
        //var prefab = ResourceManager.Instance.SyncLoad<GameObject>("Assets/GameAssets/Prefabs/ExamplePrefab1.prefab");
        //mExampleGo = GameObject.Instantiate(prefab);
        ResourceManager.Instance.AsyncLoad<GameObject>("Assets/GameAssets/Prefabs/ExamplePrefab1.prefab", (obj) =>
        {
            mExampleGo = GameObject.Instantiate(obj) as GameObject;
        });
        ResourceManager.Instance.AsyncLoad<GameObject>("Assets/GameAssets/Prefabs/ExamplePrefab2.prefab", (obj) =>
        {
            mExampleGo = GameObject.Instantiate(obj) as GameObject;
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject.Destroy(mExampleGo);
            ResourceManager.Instance.Unload("Assets/GameAssets/Prefabs/ExamplePrefab1.prefab", true);
        }
    }
}
