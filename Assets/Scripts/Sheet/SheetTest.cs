/**
 * 表格数据测试
 */

using UnityEngine;

public class SheetTest
{
    public static void Test()
    {
        var item = SheetManager.Instance.GetExample(1);
        Debug.Log(item.exampleInt);
        Debug.Log(item.exampleFloat);
        Debug.Log(item.exampleString);
        Debug.Log(item.exampleBool);
        var str = "exampleArray1: ";
        foreach (var value in item.exampleArray1)
        {
            str += value + " ";
        }
        Debug.Log(str);

        str = "exampleArray2: ";
        foreach (var value in item.exampleArray2)
        {
            str += value + " ";
        }
        Debug.Log(str);

        str = "exampleArray3: ";
        foreach (var value in item.exampleArray3)
        {
            str += value + " ";
        }
        Debug.Log(str);
    }
}
