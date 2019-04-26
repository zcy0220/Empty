/**
 * 双向链表
 * 与System.Collections.Generic.LinkedList区别：节点做了回收缓存
 */

using Base.Pool;

namespace Base.Collections
{
    public class LinkedList<T> where T : class, new()
    {
        /// <summary>
        /// 表头
        /// </summary>
        public LinkedListNode<T> First;
        /// <summary>
        /// 表尾
        /// </summary>
        public LinkedListNode<T> Last;
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { protected set; get; }
        /// <summary>
        /// 双向链表节点类对象池
        /// </summary>
        protected ClassObjectPool<LinkedListNode<T>> mDoubleLinkedListNodePool = new ClassObjectPool<LinkedListNode<T>>();

        /// <summary>
        /// 添加节点到头部
        /// </summary>
        public LinkedListNode<T> AddFirst(T t)
        {
            var node = mDoubleLinkedListNodePool.Spawn();
            node.Previous = null;
            node.Next = null;
            node.Value = t;
            return AddFirst(node);
        }

        /// <summary>
        /// 添加节点到头部
        /// </summary>
        public LinkedListNode<T> AddFirst(LinkedListNode<T> node)
        {
            if (node == null) return null;
            node.Previous = null;
            if (First == null)
            {
                First = Last = node;
            }
            else
            {
                node.Next = First;
                First.Previous = node;
                First = node;
            }
            Count++;
            return First;
        }

        /// <summary>
        /// 添加节点到尾部
        /// </summary>
        public LinkedListNode<T> AddToTail(T t)
        {
            var node = mDoubleLinkedListNodePool.Spawn();
            node.Previous = null;
            node.Next = null;
            node.Value = t;
            return AddToTail(node);
        }

        /// <summary>
        /// 添加节点到尾部
        /// </summary>
        public LinkedListNode<T> AddToTail(LinkedListNode<T> node)
        {
            if (node == null) return null;
            node.Next = null;
            if (Last == null)
            {
                Last = First = node;
            }
            else
            {
                node.Previous = Last;
                Last.Next = node;
                Last = node;
            }
            Count++;
            return Last;
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        public void Remove(LinkedListNode<T> node)
        {
            if (node == null) return;
            if (node == First) First = node.Next;
            if (node == Last) Last = node.Previous;
            if (node.Previous != null) node.Previous.Next = node.Next;
            if (node.Next != null) node.Next.Previous = node.Previous;
            node.Previous = node.Next = null;
            node.Value = null;
            Count--;
            mDoubleLinkedListNodePool.Recycle(node);
        }

        /// <summary>
        /// 把节点移动到头部
        /// </summary>
        public void MoveToFirst(LinkedListNode<T> node)
        {
            if (node == null || node == First) return;
            if (node.Previous == null && node.Next == null) return;
            if (node == Last) Last = node.Previous;
            if (node.Previous != null) node.Previous.Next = node.Next;
            if (node.Next != null) node.Next.Previous = node.Previous;
            node.Previous = null;
            node.Next = First;
            First.Previous = node;
            First = node;
        }
        
        /// <summary>
        /// 清除所有节点
        /// </summary>
        public void Clear()
        {
            while(Last != null)
            {
                Remove(Last);
            }
        }
    }
}
