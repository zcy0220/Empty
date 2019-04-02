/**
 * 资源加载测试
 */

using UnityEngine;
using UnityEngine.UI;

public class ResourceManagerTest : MonoBehaviour
{
    [SerializeField]
    private Image SyncTestImage;
    [SerializeField]
    private Image AsyncTestImage;

    private void Start()
    {
        AppConfig.UseAssetBundle = true;
        // 测试Prefab同步加载
        var prefab1 = ResourceManager.Instance.SyncLoad<GameObject>("Prefabs/ExamplePrefab1.prefab");
        GameObject.Instantiate(prefab1);
        // 测试Prefab异步加载
        ResourceManager.Instance.AsyncLoad<GameObject>("Prefabs/ExamplePrefab2.prefab", (obj) =>
        {
            GameObject.Instantiate(obj);
        });
        // 测试Sprite同步加载
        SyncTestImage.sprite = ResourceManager.Instance.SyncLoad<Sprite>("UI/Item/1001.png");
        // 测试Sprite异步加载
        ResourceManager.Instance.AsyncLoad<Sprite>("UI/Item/1002.png", (obj) =>
        {
            AsyncTestImage.sprite = obj as Sprite;
        });
    }
}
