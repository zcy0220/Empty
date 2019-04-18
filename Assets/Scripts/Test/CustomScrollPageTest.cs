/**
 * 自定义翻滚页测试
 */

using UnityEngine;
using Base.Custom;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomScrollPageTest : MonoBehaviour
{
    public CustomScrollPage ScrollPage;
    public List<Image> PointsList;
    private Image mCurPoint;
    private Color White = new Color(0.8f, 0.8f, 0.8f);
    private Color Black = new Color(0.2f, 0.2f, 0.2f);

    public void Start()
    {
        mCurPoint = PointsList[0];
        mCurPoint.color = White;
        ScrollPage.OnPageAction = (index) =>
        {
            mCurPoint.color = Black;
            mCurPoint = PointsList[index];
            mCurPoint.color = White;
        };
    }
}
