/**
 * 资源加载器
 */

public abstract class AssetLoader
{
    /// <summary>
    /// 唯一Id
    /// </summary>
    private static int mId = 0;
    //==============================================================
    public int Id { get { return mId; } }
    
    /// <summary>
    /// 构造
    /// </summary>
    protected AssetLoader() { mId++; }

    /// <summary>
    /// 同步加载
    /// </summary>
    public virtual T SyncLoad<T>(string path) where T : UnityEngine.Object
    {
        return null;
    }
}
