/**
 * Unity对象对象池
 */

using UnityEngine;
using System.Collections.Generic;

public class GameObjectPool
{
    /// <summary>
    /// 资源路径
    /// </summary>
    private string mPath;
    /// <summary>
    /// 最大值
    /// </summary>
    private int mMaxCount;
    /// <summary>
    /// 回收节点父节点
    /// </summary>
    private Transform mParent;
    /// <summary>
    /// The pool
    /// </summary>
    private Stack<GameObject> mPool = new Stack<GameObject>();
    /// <summary>
    /// 没有回收的对象个数
    /// </summary>
    public int NoRecycleCount { get; private set; }
    /// <summary>
    /// 最后一个回收对象回收的时刻
    /// </summary>
    public float NoRefTime { get; private set; }

    /// <summary>
    /// 初始化默认为不限个数
    /// </summary>
    public GameObjectPool(string path, Transform parent, int maxCount = -1)
    {
        mPath = path;
        mParent = parent;
        mMaxCount = maxCount;
        NoRecycleCount = 0;
        NoRefTime = -1.0f;
    }

    /// <summary>
    /// 同步分配对象
    /// </summary>
    public GameObject SyncSpawn()
    {
        if (mPool.Count > 0)
        {
            NoRecycleCount++;
            return mPool.Pop();
        }
        else
        {
            NoRecycleCount++;
            var orginal = ResourceManager.Instance.SyncLoad<GameObject>(mPath);
            var go = GameObject.Instantiate(orginal);
            go.name = mPath;
            return go;
        }
    }

    /// <summary>
    /// 异步加载分配对象
    /// </summary>
    /// <param name="callback"></param>
    public void AsyncSpawn(System.Action<GameObject> callback)
    {
        if (mPool.Count > 0)
        {
            NoRecycleCount++;
            callback(mPool.Pop());
        }
        else
        {
            ResourceManager.Instance.AsyncLoad<GameObject>(mPath, (orginal) =>
            {
                NoRecycleCount++;
                var go = GameObject.Instantiate(orginal) as GameObject;
                go.name = mPath;
                callback(go);
            });
        }
    }

    /// <summary>
    /// 回收对象
    /// </summary>
    public void Recycle(GameObject obj)
    {
        NoRecycleCount--;

        if (NoRecycleCount == 0) NoRefTime = Time.realtimeSinceStartup;

        if (mMaxCount > 0 && mPool.Count >= mMaxCount)
        {
            GameObject.Destroy(obj);
            return;
        }
        obj.transform.SetParent(mParent);
        mPool.Push(obj);
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public void Dispose()
    {
        foreach (var obj in mPool)
        {
            GameObject.Destroy(obj);
        }
        mPool.Clear();
        ResourceManager.Instance.Unload(mPath, false, NoRecycleCount + mPool.Count);
    }
}
