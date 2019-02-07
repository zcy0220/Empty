/**
 * 双向链表
 */

using Base.Pool;

namespace Base.Collections
{
    public class DoubleLinkedList<T> where T : class, new()
    {
        /// <summary>
        /// 表头
        /// </summary>
        public DoubleLinkedListNode<T> Head;
        /// <summary>
        /// 表尾
        /// </summary>
        public DoubleLinkedListNode<T> Tail;
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { protected set; get; }
        /// <summary>
        /// 双向链表节点类对象池
        /// </summary>
        protected ClassObjectPool<DoubleLinkedListNode<T>> mDoubleLinkedListNodePool = new ClassObjectPool<DoubleLinkedListNode<T>>();

        /// <summary>
        /// 添加节点到头部
        /// </summary>
        public DoubleLinkedListNode<T> AddToHead(T t)
        {
            var node = mDoubleLinkedListNodePool.Spawn();
            node.Prev = null;
            node.Next = null;
            node.Current = t;
            return AddToHead(node);
        }

        /// <summary>
        /// 添加节点到头部
        /// </summary>
        public DoubleLinkedListNode<T> AddToHead(DoubleLinkedListNode<T> node)
        {
            if (node == null) return null;
            node.Prev = null;
            if (Head == null)
            {
                Head = Tail = node;
            }
            else
            {
                node.Next = Head;
                Head.Prev = node;
                Head = node;
            }
            Count++;
            return Head;
        }

        /// <summary>
        /// 添加节点到尾部
        /// </summary>
        public DoubleLinkedListNode<T> AddToTail(T t)
        {
            var node = mDoubleLinkedListNodePool.Spawn();
            node.Prev = null;
            node.Next = null;
            node.Current = t;
            return AddToTail(node);
        }

        /// <summary>
        /// 添加节点到尾部
        /// </summary>
        public DoubleLinkedListNode<T> AddToTail(DoubleLinkedListNode<T> node)
        {
            if (node == null) return null;
            node.Next = null;
            if (Tail == null)
            {
                Tail = Head = node;
            }
            else
            {
                node.Prev = Tail;
                Tail.Next = node;
                Tail = node;
            }
            Count++;
            return Tail;
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        public void RemoveNode(DoubleLinkedListNode<T> node)
        {
            if (node == null) return;
            if (node == Head) Head = node.Next;
            if (node == Tail) Tail = node.Prev;
            if (node.Prev != null) node.Prev.Next = node.Next;
            if (node.Next != null) node.Next.Prev = node.Prev;
            node.Prev = node.Next = null;
            node.Current = null;
            Count--;
            mDoubleLinkedListNodePool.Recycle(node);
        }

        /// <summary>
        /// 把节点移动到头部
        /// </summary>
        public void MoveToHead(DoubleLinkedListNode<T> node)
        {
            if (node == null || node == Head) return;
            if (node.Prev == null && node.Next == null) return;
            if (node == Tail) Tail = node.Prev;
            if (node.Prev != null) node.Prev.Next = node.Next;
            if (node.Next != null) node.Next.Prev = node.Prev;
            node.Prev = null;
            node.Next = Head;
            Head.Prev = node;
            Head = node;
        }
    }
}
