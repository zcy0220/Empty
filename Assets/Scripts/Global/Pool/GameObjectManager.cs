/**
 * Unity 游戏对象池
 */

using UnityEngine;
using Base.Common;
using System.Collections.Generic;

public class GameObjectManager : MonoSingleton<GameObjectManager>
{
    /// <summary>
    /// 资源路径 -> UnityObjectPool
    /// </summary>
    private Dictionary<string, GameObjectPool> mPoolDict = new Dictionary<string, GameObjectPool>();

    /// <summary>
    /// 构造
    /// </summary>
    public GameObjectManager()
    {
        //有上限要求的可以在初始化里创建好
    }
    
    /// <summary>
    /// 获得游戏对象池
    /// </summary>
    /// <returns></returns>
    public GameObjectPool GetGameObjectPool(string path)
    {
        GameObjectPool pool = null;
        if (!mPoolDict.TryGetValue(path, out pool))
        {
            // todo 这里默认的对象池都没上限
            pool = new GameObjectPool(path, transform);
            mPoolDict.Add(path, pool);
        }
        return pool;
    }

    /// <summary>
    /// 同步分配游戏对象
    /// </summary>
    public GameObject SyncSpawn(string path)
    {
        var pool = GetGameObjectPool(path);
        return pool.SyncSpawn();
    }

    /// <summary>
    /// 异步分配游戏对象
    /// </summary>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    public void AsyncSpawn(string path, System.Action<GameObject> callback)
    {
        var pool = GetGameObjectPool(path);
        pool.AsyncSpawn(callback);
    }
    
    /// <summary>
    /// 回收
    /// </summary>
    public void Recycle(GameObject obj)
    {
        var pool = GetGameObjectPool(obj.name);
        pool.Recycle(obj);
    }

    /// <summary>
    /// 施放指定路径的对象池资源
    /// </summary>
    public void DisposeGameObjectPool(string path)
    {
        var pool = GetGameObjectPool(path);
        pool.Dispose();
    }

    /// <summary>
    /// 加载好资源，并初始化GameObject，放入缓存池
    /// </summary>
    /// <param name="path"></param>
    public void PreLoadGameObject(string path, System.Action callback)
    {
        var pool = GetGameObjectPool(path);
        pool.AsyncSpawn((obj) =>
        {
            pool.Recycle(obj);
            callback();
        });
    }
}
