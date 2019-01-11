/**
 * 对象池管理
 */

using System;
using Base.Common;
using System.Collections.Generic;

namespace Base.Pool
{
    public class PoolManager : Singleton<PoolManager>
    {
        private const int MAX_SIZE = 20;
        /// <summary>
        /// 对象队列集合
        /// </summary>
        private Dictionary<Type, Queue<object>> mPoolDict = new Dictionary<Type, Queue<object>>();
        /// <summary>
        /// 创建委托集合
        /// </summary>
        private Dictionary<Type, Creater> mCreaterDict = new Dictionary<Type, Creater>();
        /// <summary>
        /// 创建委托
        /// </summary>
        public delegate object Creater();

        /// <summary>
        /// 获得对应对象池
        /// </summary>
        private Queue<object> GetPool(Type type)
        {
            if (mPoolDict.ContainsKey(type))
            {
                return mPoolDict[type];
            }
            else
            {
                var pool = new Queue<object>();
                mPoolDict.Add(type, pool);
                return pool;
            }
        }
        
        /// <summary>
        /// 注册创建委托
        /// </summary>
        public void RegistCreater<T>(Creater creater) where T : class
        {
            var type = typeof(T);
            if (mCreaterDict.ContainsKey(type))
                mCreaterDict[type] = creater;
            else
                mCreaterDict.Add(type, creater);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        public T Fetch<T>() where T : class, new()
        {
            var type = typeof(T);
            var queue = GetPool(type);
            if (queue.Count > 0) return queue.Dequeue() as T;
            if (mCreaterDict.ContainsKey(type)) return mCreaterDict[type] as T;
            return new T();
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        public void Recycle<T>(T obj) where T : class
        {
            if (obj == null) return;
            var type = obj.GetType();
            var queue = GetPool(type);
            if (queue.Contains(obj)) return;
            if (queue.Count > MAX_SIZE) return;
            queue.Enqueue(obj);
        }
    }
}
