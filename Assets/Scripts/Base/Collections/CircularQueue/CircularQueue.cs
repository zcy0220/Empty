/**
 * 环形队列
 * 固定容量的FIFO队列
 */

using System.Collections;
using System.Collections.Generic;

namespace Base.Collections
{
    public class CircularQueue<T> : IEnumerable<T>
    {
        /// <summary>
        /// 队列数组
        /// </summary>
        protected T[] mQueue;
        /// <summary>
        /// 队列第一个索引
        /// </summary>
        protected int mHead;
        /// <summary>
        /// 队列最后一个索引
        /// </summary>
        protected int mTail;
        /// <summary>
        /// 队列个数
        /// </summary>
        public int Count { get { return mTail >= mHead ? mTail - mHead : Capacity; } }
        /// <summary>
        /// 队列容量
        /// </summary>
        public int Capacity { get { return mQueue.Length; } }
        /// <summary>
        /// 满状态判断
        /// </summary>
        public bool IsFull { get { return Count == Capacity; } }
        /// <summary>
        /// 空状态判断
        /// </summary>
        public bool IsEmpty { get { return Count == 0; } }
        
        /// <summary>
        /// 构造
        /// </summary>
        public CircularQueue(int capacity)
        {
            mQueue = new T[capacity];
            mHead = mTail = 0;
        }

        /// <summary>
        /// 插入一个元素
        /// 这里为了简单处理，选择的策略是丢弃溢出数据
        /// 另一个策略是，用新数据覆盖Head的数据
        /// 个人认为，当循环队列处于阻塞状态时，是丢弃还是覆盖都不重要了(还是针对实际项目处理最好- -!)
        /// </summary>
        public void Enqueue(T item)
        {
            if (IsFull) return;
            mQueue[mTail] = item;
            mTail = (++mTail) % Capacity;
        }

        /// <summary>
        /// 弹出尾部元素
        /// </summary>
        public T Dequeue()
        {
            if (IsEmpty) return default(T);
            var item = mQueue[mHead];
            mHead = (++mHead) % Capacity;
            return item;
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            mHead = 0;
            mTail = 0;
        }

        /// <summary>
        /// GetEnumerator
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        /// <summary>
        /// GetEnumerator
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 索引访问
        /// </summary>
        public T this[int index]
        {
            get
            {
                return mQueue[(mHead + index) % Capacity];
            }
        }
    }
}
