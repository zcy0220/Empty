/**
 * 类对象池
 */

using System.Collections.Generic;

namespace Base.Pool
{
    public class ClassObjectPool<T> where T : class, new()
    {
        /// <summary>
        /// The pool
        /// </summary>
        protected Stack<T> mPool = new Stack<T>();
        /// <summary>
        /// 最大对象个数，-1表示不限个数
        /// </summary>
        protected int mMaxCount = 0;
        /// <summary>
        /// 没有回收的对象个数
        /// </summary>
        protected int mNoRecycleCount = 0;

        /// <summary>
        /// 初始化默认为不限个数
        /// </summary>
        public ClassObjectPool(int maxCount = -1)
        {
            mMaxCount = maxCount;
        }

        /// <summary>
        /// 分配类对象
        /// </summary>
        public T Spawn()
        {
            mNoRecycleCount++;
            if (mPool.Count > 0)
            {
                T obj = mPool.Pop();
                if (obj == null)
                {
                    obj = new T();
                }
                return obj;
            }
            else
            {
                T obj = new T();
                return obj;
            }
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        public bool Recycle(T obj)
        {
            if (obj == null) return false;
            mNoRecycleCount--;

            if (mPool.Count >= mMaxCount && mMaxCount > 0)
            {
                obj = null;
                return false;
            }

            mPool.Push(obj);
            return true;
        }
    }
}

