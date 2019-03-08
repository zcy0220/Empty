/**
 * 场景基类
 */

public abstract class BaseScene
{
    /// <summary>
    /// 场景ID
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 场景名
    /// </summary>
    public string Name { get; private set; }


    public void Init(int id, string name)
    {

    }
}
