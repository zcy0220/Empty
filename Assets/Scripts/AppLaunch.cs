/**
 * 程序启动项
 */

using UnityEngine;

public class AppLaunch : MonoBehaviour
{
    private void Awake()
    {
        AssetBundleManager.Instance.LoadAssetBundleConfig();
    }
}
