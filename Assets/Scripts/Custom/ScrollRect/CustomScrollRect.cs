/**
 * 自定义ScrollRect
 * 无限循环使用
 */

using UnityEngine;
using UnityEngine.UI;
using Base.Debug;
using System.Collections.Generic;
using Base.Extension;
using System;

public class CustomScrollRect : MonoBehaviour
{
    /// <summary>
    /// 依附的滚动列表
    /// </summary>
    public ScrollRect Scroller;
    /// <summary>
    /// 子项Prefab
    /// </summary>
    public GameObject CellPrefab;
    /// <summary>
    /// 子项间隔
    /// </summary>
    public int CellPadiding = 0;
    /// <summary>
    /// Cell宽高
    /// </summary>
    public float CellWidth = 100;
    public float CellHeight = 100;
    /// <summary>
    /// 总数
    /// </summary>
    private int mTotal = 0;
    /// <summary>
    /// Scroller.content
    /// </summary>
    private RectTransform mContent;
    /// <summary>
    /// 加载的数量，一般比可视个数大2个
    /// </summary>
    private int mViewCount = 1;
    /// <summary>
    /// 单行或单列的Cell数量
    /// </summary>
    private int mPerLineCellNum = 1;
    /// <summary>
    /// 排列方式
    /// </summary>
    private bool mHorizontal;
    /// <summary>
    /// Content当前索引
    /// </summary>
    private int mIndex = -1;
    /// <summary>
    /// 可视的ItemList
    /// </summary>
    private List<CustomScrollRectIndex> mItemList;
    /// <summary>
    /// 保留没用的Item
    /// </summary>
    private Queue<CustomScrollRectIndex> mUnUsedQueue;
    /// <summary>
    /// 总数变化更新Size
    /// </summary>
    public int Total { get { return mTotal; } set { mTotal = value; UpdateTotalSize(); } }
    /// <summary>
    /// 抛出刷新Item数据委托
    /// </summary>
    public Action<CustomScrollRectIndex> UpdateItem;

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void Awake()
    {
        mContent = Scroller.content;
        mHorizontal = Scroller.horizontal;
        Scroller.onValueChanged.AddListener(OnValueChanged);
        var size = Scroller.GetComponent<RectTransform>().sizeDelta;
        mViewCount = mHorizontal ? Mathf.CeilToInt(size.x / (CellWidth + CellPadiding)) + 1 : Mathf.CeilToInt(size.y / (CellHeight + CellPadiding)) + 1;
        mPerLineCellNum = mHorizontal ? Mathf.FloorToInt(size.y / CellHeight) : Mathf.FloorToInt(size.x / CellWidth);
        mItemList = new List<CustomScrollRectIndex>();
        mUnUsedQueue = new Queue<CustomScrollRectIndex>();
    }
    
    /// <summary>
    /// 滑动
    /// </summary>
    public void OnValueChanged(Vector2 pos)
    {
        var index = GetPosIndex();
        if (mIndex != index && index > -1)
        {
            mIndex = index;
            // 处理可视窗口外的Item
            DealOutViewItem();
            // 处理可视窗口内的Item
            for (var i = mIndex * mPerLineCellNum; i < (mIndex + mViewCount) * mPerLineCellNum; i++)
            {
                if (i < 0) continue;
                if (i > mTotal - 1) continue;
                bool inView = false;
                foreach (var item in mItemList)
                {
                    if (item.Index == i) inView = true;
                }
                if (inView) continue;
                CreateItem(i);
            }
        }
    }

    /// <summary>
    /// 提供给外部的方法，添加指定位置的Item
    /// </summary>
    public void AddItem(int index)
    {
        if (index > mTotal)
        {
            Debugger.LogError("Add Error Index: " + index);
            return;
        }
        Total++;
        AddItemIntoContent(index);
    }
    
    /// <summary>
    /// 添加Item到Content
    /// </summary>
    /// <param name="index"></param>
    private void AddItemIntoContent(int index)
    {
        for (var i = 0; i < mItemList.Count; i++)
        {
            var item = mItemList[i];
            if (item.Index >= index) item.Index++;
            else item.Index = item.Index;
        }
        CreateItem(index, false);
    }
    
    /// <summary>
    /// 创建Item
    /// </summary>
    /// <param name="index"></param>
    /// <param name="auto">是否自动添加</param>
    private void CreateItem(int index, bool auto = true)
    {
        CustomScrollRectIndex item;
        if (mUnUsedQueue.Count > 0)
        {
            item = mUnUsedQueue.Dequeue();
        }
        else
        {
            var go = GameObject.Instantiate(CellPrefab);
            go.transform.SetParent(mContent, false);
            item = go.GetOrAddComponent<CustomScrollRectIndex>();
        }
        item.Scroller = this;
        item.Index = index;
        mItemList.Add(item);
        if (UpdateItem != null) UpdateItem(item);

        // 手动添加的Item, 超出可视框要回收
        if (!auto) DealOutViewItem();
    }

    /// <summary>
    /// 处理超出可视框的Item
    /// </summary>
    private void DealOutViewItem()
    {
        var index = GetPosIndex();
        for (var i = mItemList.Count; i > 0; i--)
        {
            var item = mItemList[i - 1];
            if (item.Index < index * mPerLineCellNum || (item.Index >= (index + mViewCount) * mPerLineCellNum))
            {
                mItemList.Remove(item);
                mUnUsedQueue.Enqueue(item);
            }
        }
    }

    /// <summary>
    /// 删除Item
    /// </summary>
    /// <param name="index"></param>
    public void DeleteItem(int index)
    {
        if (index < 0 || index > mTotal - 1)
        {
            Debugger.LogError("Delete Error Index: " + index);
            return;
        }
        Total--;
        DeleteItemFromContent(index);
    }
    
    /// <summary>
    /// Content中删除
    /// </summary>
    /// <param name="index"></param>
    public void DeleteItemFromContent(int index)
    {
        var max = -1;
        for (var i = mItemList.Count; i > 0; i--)
        {
            var item = mItemList[i - 1];
            if (item.Index == index)
            {
                GameObject.Destroy(item.gameObject);
                mItemList.Remove(item);
            }
            if (item.Index > max)
            {
                max = item.Index;
            }
            if (item.Index > index)
            {
                item.Index--;
            }
            if (item.Index < index)
            {
                item.Index = item.Index;
            }
        }
        if (max < Total)
        {
            CreateItem(max);
        }
    }

    /// <summary>
    /// Content当前位置对于的Index
    /// </summary>
    private int GetPosIndex()
    {
        if (mHorizontal)
        {
            return Mathf.FloorToInt(mContent.anchoredPosition.x / -(CellWidth + CellPadiding));
        }
        else
        {
            return Mathf.FloorToInt(mContent.anchoredPosition.y / (CellHeight + CellPadiding));
        }
    }

    /// <summary>
    /// 根据指定Index返回其在Content中的位置
    /// 这里的index为Item的索引
    /// </summary>
    public Vector3 GetPosition(int index)
    {
        if (mHorizontal)
        {
            return new Vector3((CellWidth + CellPadiding) * (index / mPerLineCellNum) + CellWidth * 0.5f, -(CellHeight + CellPadiding) * (index % mPerLineCellNum) - CellHeight* 0.5f, 0);
        }
        else
        {
            return new Vector3(CellWidth * (index % mPerLineCellNum) + CellWidth * 0.5f, -(CellHeight + CellPadiding) * (index / mPerLineCellNum) - CellHeight * 0.5f, 0);
        }
    }

    /// <summary>
    /// 更新Content总的Size
    /// </summary>
    private void UpdateTotalSize()
    {
        int lineCount = Mathf.CeilToInt(1.0f * mTotal / mPerLineCellNum);
        if (mHorizontal)
        {
            mContent.sizeDelta = new Vector2(CellWidth * lineCount + CellPadiding * (lineCount - 1), mContent.sizeDelta.y);
        }
        else
        {
            mContent.sizeDelta = new Vector2(mContent.sizeDelta.x, CellHeight * lineCount + CellPadiding * (lineCount - 1));
        }
    }
}
