/**
 * 双向链表节点
 */

namespace Base.Collections
{
    public class LinkedListNode<T> where T : class, new()
    {
        /// <summary>
        /// 前置节点
        /// </summary>
        public LinkedListNode<T> Previous;
        /// <summary>
        /// 后置节点
        /// </summary>
        public LinkedListNode<T> Next;
        /// <summary>
        /// 当前节点
        /// </summary>
        public T Value;
    }
}