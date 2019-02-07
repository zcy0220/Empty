/**
 * 资源管理列表
 */

using Base.Collections;
using System.Collections.Generic;

public class ResourceMapList<T> where T : class, new()
{
    private DoubleLinkedList<T> mDoubleLinkedList = new DoubleLinkedList<T>();
    private Dictionary<T, DoubleLinkedListNode<T>> mDoubleLinkedListNodeDict = new Dictionary<T, DoubleLinkedListNode<T>>();
    //==========================================================================
    public T Tail { get { return mDoubleLinkedList.Tail == null ? null : mDoubleLinkedList.Tail.Current; } }
    public int Count { get { return mDoubleLinkedListNodeDict.Count; } }

    ~ResourceMapList()
    {
        Clear();
    }

    /// <summary>
    /// 添加到头部
    /// </summary>
    public void AddToHead(T t)
    {
        DoubleLinkedListNode<T> node = null;
        if (mDoubleLinkedListNodeDict.TryGetValue(t, out node) && node != null)
        {
            mDoubleLinkedList.AddToHead(node);
            return;
        }
        mDoubleLinkedList.AddToHead(t);
        mDoubleLinkedListNodeDict.Add(t, mDoubleLinkedList.Head);
    }

    /// <summary>
    /// 表尾弹出一个节点
    /// </summary>
    /// <returns>The pop.</returns>
    public void Pop()
    {
        if (mDoubleLinkedList.Tail != null)
        {
            Remove(mDoubleLinkedList.Tail.Current);
        }
    }

    /// <summary>
    /// 删除某个节点
    /// </summary>
    public void Remove(T t)
    {
        DoubleLinkedListNode<T> node = null;
        if (!mDoubleLinkedListNodeDict.TryGetValue(t, out node) || node == null) return;
        mDoubleLinkedList.RemoveNode(node);
        mDoubleLinkedListNodeDict.Remove(t);
    }

    /// <summary>
    /// 查找对象
    /// </summary>
    public bool Find(T t)
    {
        DoubleLinkedListNode<T> node = null;
        if (!mDoubleLinkedListNodeDict.TryGetValue(t, out node) || node == null) return false;
        return true;
    }

    /// <summary>
    /// 把节点移到头部
    /// </summary>
    public bool MoveToHead(T t)
    {
        DoubleLinkedListNode<T> node = null;
        if (!mDoubleLinkedListNodeDict.TryGetValue(t, out node) || node == null) return false;
        mDoubleLinkedList.MoveToHead(node);
        return true;
    }

    /// <summary>
    /// 清空链表
    /// </summary>
    public void Clear()
    {
        while(mDoubleLinkedList.Tail != null)
        {
            Remove(mDoubleLinkedList.Tail.Current);
        }
    }
}
