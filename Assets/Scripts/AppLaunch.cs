/**
 * 程序启动项
 */

using UnityEngine;

public class AppLaunch : MonoBehaviour
{
    /// <summary>
    /// 正式环境下，游戏启动初始化
    /// </summary>
    private void Awake()
    {
        SceneController.Instance.LoadScene(new StartScene(), true);
    }
}
