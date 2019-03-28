/**
 * 搭配自定义ScrollRect使用的索引
 * 添加到CustomScrollRect中的Item要挂该组件
 */

using UnityEngine;

public class CustomScrollRectIndex : MonoBehaviour
{
    /// <summary>
    /// 索引
    /// </summary>
    private int mIndex;
    /// <summary>
    /// 父类的Scroll
    /// </summary>
    public CustomScrollRect Scroller { set; get; }

    /// <summary>
    /// 修改索引的话，修改对于的位置
    /// </summary>
    public int Index
    {
        get { return mIndex; }
        set
        {
            mIndex = value;
            transform.localPosition = Scroller.GetPosition(mIndex);
        }
    }
}
