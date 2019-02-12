/**
 * 程序启动项
 */

using UnityEngine;

public class AppLaunch : MonoBehaviour
{
    private void Awake()
    {
        AppConfig.UseAssetBundle = true;
        AssetBundleManager.Instance.LoadAssetBundleConfig();
    }
}
