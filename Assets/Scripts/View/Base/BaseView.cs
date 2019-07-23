/**
 * 基础视图
 */

using UnityEngine;

public abstract class BaseView
{
    /// <summary>
    /// 
    /// </summary>
    private static int ID;
    /// <summary>
    /// 对应的ViewPrefab
    /// </summary>
    private GameObject mGameObject;
    /// <summary>
    /// 唯一Id
    /// </summary>
    public int UniqueId { get; private set; }
    
    /// <summary>
    /// 构造
    /// </summary>
    public BaseView()
    {
        UniqueId = ID++;
    }

    /// <summary>
    /// 创建
    /// </summary>
    public virtual void Create(GameObject gameObject, params object[] data)
    {
        mGameObject = gameObject;
        Enter();
    }

    /// <summary>
    /// 进入
    /// </summary>
    public virtual void Enter() { }

    /// <summary>
    /// 退出
    /// </summary>
    public virtual void Exit() { }

    /// <summary>
    /// 销毁
    /// </summary>
    public virtual void Dispose()
    {
        Exit();
        if (mGameObject != null) GameObject.Destroy(mGameObject);
    }
    
    /// <summary>
    /// 设置层级
    /// </summary>
    public virtual void SetSiblingIndex(int siblingIndex)
    {
        if (mGameObject != null)
        {
            mGameObject.transform.SetSiblingIndex(siblingIndex);
        }
    }
}
