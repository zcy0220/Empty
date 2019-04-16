# 代码规范示例

## C#代码规范
~~~c#
/**
 * 程序说明
 */

/// <summary>
/// 命名空间
/// </summary>
using UnityEngine;

public class Example : MonoBehaviour
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

## Lua代码规范
~~~lua
-- 系统功能扩展：保持与原功能的风格一致，使用小写，如string的拓展：string.split
-- 逻辑功能命名：保持与C#代码规范一致，采用大写驼峰式命名
--------------------------------示例---------------------------------------
--[[
    @desc: 程序说明
]]

local Example = Class("Example")

-- 方法说明（全部为局部）
local function ExampleFunc()
end

-- 方法导出（都定义在底部）
Example.ExampleFunc = ExampleFunc

return Example
~~~