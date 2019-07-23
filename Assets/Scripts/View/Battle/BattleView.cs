/**
 * 战斗页面
 */

using UnityEngine;
using UnityEngine.UI;

public class BattleView : BaseView, IEventDispatcher
{
    /// <summary>
    /// 创建
    /// </summary>
    public override void Create(GameObject gameObject, params object[] data)
    {
        base.Create(gameObject, data);
        var btnBack = gameObject.transform.Find("BtnBack").GetComponent<Button>();
        btnBack.onClick.AddListener(OnBtnBack);
    }

    /// <summary>
    /// 返回
    /// </summary>
    private void OnBtnBack()
    {
        SceneController.Instance.LoadScene(new MainScene());
    }
}
