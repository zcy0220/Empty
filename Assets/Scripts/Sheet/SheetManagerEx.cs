/**
 * SheetManager的拓展功能
 */

using UnityEngine;
using Base.Common;
using Base.Utils;
using Base.Debug;

public partial class SheetManager : Singleton<SheetManager>
{
    public const string PREFIX = "Assets/GameAssets/Sheets/";
    public const string POSTFIX = ".bytes";
    /// <summary>
    /// 获得表格数据
    /// </summary>
    public T GetSheetInfo<T>(string fileName)
    {
        var text = ResourceManager.Instance.SyncLoad<TextAsset>(StringUtil.Concat(PREFIX, fileName, POSTFIX));
        return ProtobufUtil.NDeserialize<T>(text.bytes);
    }

    /// <summary>
    /// 测试用例
    /// </summary>
    public void Test()
    {
        var item = GetExample(1);
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
