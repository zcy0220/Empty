# 代码规范示例

~~~c#
/**
 * 程序说明
 */

/// <summary>
/// 命名空间
/// </summary>
using UnityEngine;

public class ProgramingStyle : MonoBehaviour
{
    #region Abbreviation
    /**
     * GameObject->Go
     * Transform->Trans
     * Position->Pos
     * Button->Btn
     * Dictionary->Dict
     * Number->Num
     * Current->Cur
     */
    #endregion

    /// <summary>
    /// 私有最好也别省略private
    /// 私有变量可以加前缀 m 表示私有成员 mExampleBtn
    /// 但在Unity进行私有序列化时[SerializeField] 可视面板上的 M 就比较奇怪
    /// </summary>
    private Button mExampleBtn;
    /// <summary>
    /// 公有变量和公共属性使用首字母大写驼峰式
    /// </summary>
    public int ExampleNum;
    public int ExampleIndex { get; set; }

    /// <summary>
    /// 方法名一律使用首字母大写驼峰式
    /// </summary>
    public void ExampleFunc()
    {
        //局部变量最好用var匿名声明 小写驼峰式
        var go = transform.Find("Example").gameObject
    }
}
~~~