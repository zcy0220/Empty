/**
 * 双向链表节点
 */

namespace Base.Collections
{
    public class DoubleLinkedListNode<T> where T : class, new()
    {
        /// <summary>
        /// 前置节点
        /// </summary>
        public DoubleLinkedListNode<T> Prev;
        /// <summary>
        /// 后置节点
        /// </summary>
        public DoubleLinkedListNode<T> Next;
        /// <summary>
        /// 当前节点
        /// </summary>
        public T Current;
    }
}