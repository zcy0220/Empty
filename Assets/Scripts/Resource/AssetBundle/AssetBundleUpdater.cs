/**
 * AssetBundle更新下载器
 */

using UnityEngine;
using System.Collections;

public class AssetBundleUpdater : MonoBehaviour
{
    /// <summary>
    /// 本地版本配置信息
    /// </summary>
    private string mLocalAppVersion;
    /// <summary>
    /// 服务器版本配置信息
    /// </summary>
    private string mServerAppVersion;

    /// <summary>
    /// 开始更新
    /// </summary>
    private void Start()
    {
        StartCoroutine(CheckUpdate());
    }

    /// <summary>
    /// 检查更新，包括大版本换包和热更新
    /// </summary>
    private IEnumerator CheckUpdate()
    {
        // 是否开启热更检测
        if (!AppConfig.CheckVersionUpdate)
        {
            StartGame();
            yield break;
        }
        // 加载并初始化版本信息文件
        yield return InitVersion();
        
        yield return null;
    }

    /// <summary>
    /// 初始化本地版本
    /// </summary>
    IEnumerator InitVersion()
    {
        yield return null;
    }

    /// <summary>
    /// 正式开始游戏
    /// </summary>
    private void StartGame()
    {
        ScenesManager.Instance.LoadSceneSync("Main");
    }
}
