/**
 * 主界面视图
 */

using UnityEngine;
using UnityEngine.UI;

public class MainView : BaseView
{
    /// <summary>
    /// 创建
    /// </summary>
    public override void Create(GameObject gameObject, params object[] data)
    {
        base.Create(gameObject, data);
        var btnStart = gameObject.transform.Find("BtnStart").GetComponent<Button>();
        btnStart.onClick.AddListener(OnBtnStart);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    private void OnBtnStart()
    {
        SceneController.Instance.LoadScene(new BattleScene(101));
    }
}
