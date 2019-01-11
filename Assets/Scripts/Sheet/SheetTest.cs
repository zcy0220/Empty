/**
 * 表格数据测试
 */

using Base.Debug;

public class SheetTest
{
    public static void Test()
    {
        var item = SheetManager.Instance.GetExample(1);
        Debugger.Log(item.exampleInt);
        Debugger.Log(item.exampleFloat);
        Debugger.Log(item.exampleString);
        Debugger.Log(item.exampleBool);
        var str = "exampleArray1: ";
        foreach (var value in item.exampleArray1)
        {
            str += value + " ";
        }
        Debugger.Log(str);

        str = "exampleArray2: ";
        foreach (var value in item.exampleArray2)
        {
            str += value + " ";
        }
        Debugger.Log(str);

        str = "exampleArray3: ";
        foreach (var value in item.exampleArray3)
        {
            str += value + " ";
        }
        Debugger.Log(str);
    }
}
