/**
 * UI视图管理器
 */

using UnityEngine;
using Base.Common;
using System.Collections.Generic;

public class ViewManager : MonoSingleton<ViewManager>
{
    /// <summary>
    /// 视图根节点
    /// </summary>
    private RectTransform mCanvas;
    /// <summary>
    /// Canvas路径
    /// </summary>
    private const string CANVASPATH = "Prefabs/Views/Common/Canvas.prefab";
    /// <summary>
    /// 视图存储
    /// </summary>
    private Dictionary<int, BaseView> mViewDict = new Dictionary<int, BaseView>();
    /// <summary>
    /// 视图堆栈
    /// </summary>
    private Stack<BaseView> mViewStack = new Stack<BaseView>();
    /// <summary>
    /// 当前层级
    /// </summary>
    private int mCurSiblingIndex = 0;
    //===============================================================
    public Camera UICamera { get; private set; }

    /// <summary>
    /// 初始化Canvas
    /// </summary>
    private void Awake()
    {
        ViewConfig.Init();
        var canvas = GameObjectManager.Instance.SyncSpawn(CANVASPATH);
        canvas.transform.SetParent(transform, false);
        mCanvas = canvas.GetComponent<RectTransform>();
        UICamera = mCanvas.transform.Find("UICamera").GetComponent<Camera>();
    }

    /// <summary>
    /// 创建View
    /// </summary>
    private BaseView Create<T>(params object[] data) where T : BaseView, new()
    {
        var path = ViewConfig.GetViewPath(typeof(T));
        var go = GameObjectManager.Instance.SyncSpawn(path);
        go.transform.SetParent(mCanvas, false);
        var view = new T();
        view.Create(go, data);
        mViewDict.Add(view.UniqueId, view);
        return view;
    }
    
    /// <summary>
    /// 入栈
    /// </summary>
    public void Push<T>(params object[] data) where T : BaseView, new()
    {
        // 触发当前层的Exit 
        if (mViewStack.Count > 0)
        {
            var curView = mViewStack.Peek();
            curView.Exit();
        }
        mCurSiblingIndex++;
        var newView = Create<T>(data);
        newView.SetSiblingIndex(ViewSiblingIndex.LOW + mCurSiblingIndex);
        mViewStack.Push(newView);
    }
    
    /// <summary>
    /// 退出栈顶View
    /// </summary>
    public void Pop(bool clear = false)
    {
        if (mViewStack.Count == 0) return;
        mCurSiblingIndex--;
        var curView = mViewStack.Pop();
        Close(curView.UniqueId);
        curView.Dispose();
        
        // 不是强制清除的就触发上一层的Enter
        if (clear && mViewStack.Count > 0)
        {
            var lastView = mViewStack.Peek();
            lastView.Enter();
        }
    }

    /// <summary>
    /// 替换当前View
    /// </summary>
    public void Replace<T>(params object[] data) where T : BaseView, new()
    {
        Pop(true);
        Push<T>(data);
    }

    /// <summary>
    /// 强制打开指定界面，删除栈内所有UI
    /// </summary>
    public void Open<T>(params object[] data) where T : BaseView, new()
    {
        while (mViewStack.Count > 0)
        {
            Pop(true);
        }
        Push<T>(data);
    }

    /// <summary>
    /// 关闭页面
    /// </summary>
    public void Close<T>() where T : BaseView
    {
        var type = typeof(T);
        var removeList = new List<BaseView>();
        foreach (var view in mViewDict.Values)
        {
            if (type == view.GetType())
            {
                removeList.Add(view);
            }
        }
        foreach (var view in removeList)
        {
            Close(view.UniqueId);
            view.Dispose();
        }
    }

    /// <summary>
    /// 根据Id删除UI视图
    /// </summary>
    /// <param name="uniqueId"></param>
    private void Close(int uniqueId)
    {
        if (mViewDict.ContainsKey(uniqueId))
        {
            mViewDict.Remove(uniqueId);
        }
    }

    /// <summary>
    /// 按指定层级添加UI
    /// 直接添加的View不进入Stack管理
    /// </summary>
    public void Add<T>(int siblingIndex) where T : BaseView, new()
    {
        var view = Create<T>();
        view.SetSiblingIndex(siblingIndex);
    }
}
