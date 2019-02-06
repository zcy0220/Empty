/**
 * 对象管理类
 */

using System;
using Base.Common;
using System.Collections.Generic;

public class ObjectManager : Singleton<ObjectManager>
{
    /// <summary>
    /// 存储类对象池的字典
    /// </summary>
    protected Dictionary<Type, object> mClassPoolDict = new Dictionary<Type, object>();

    /// <summary>
    /// 获取类对象池
    /// </summary>
    public ClassObjectPool<T> GetOrCreateClassPool<T>(int maxCount = -1) where T : class, new()
    {
        var type = typeof(T);
        if (!mClassPoolDict.ContainsKey(type))
        {
            var pool = new ClassObjectPool<T>(maxCount);
            mClassPoolDict.Add(type, pool);
            return pool;
        }
        return mClassPoolDict[type] as ClassObjectPool<T>;
    }
}