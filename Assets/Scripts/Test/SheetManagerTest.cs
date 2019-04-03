/**
 * 表格数据读取测试
 */

using UnityEngine;
using Base.Debug;

public class SheetManagerTest : MonoBehaviour
{
    private void Start()
    {
        AppConfig.UseAssetBundle = true;
        var item = SheetManager.Instance.GetExample(1);
        Debugger.Log("exampleInt: " + item.exampleInt);
        Debugger.Log("exampleFloat: " + item.exampleFloat);
        Debugger.Log("exampleString: " + item.exampleString);
        Debugger.Log("exampleBool: " + item.exampleBool);
        Debugger.Log("exampleClient: " + item.exampleClient);
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
