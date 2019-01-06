/**
 * 程序主入口
 */

using Sheet;
using ProtoBuf;
using UnityEngine;
using System.IO;
using Base.Utils;

public class AppMain : MonoBehaviour
{
    /// <summary>
    /// 开始游戏，初始化配置文件
    /// </summary>
    private void Awake()
    {
    }

    private void Start()
    {
        var text = Resources.Load<TextAsset>("Sheets/Example");
        var ss = ProtobufUtil.NDeserialize<ExampleArray>(text.bytes);
        var item = ss.ExampleList[0];
        Debug.Log(item.exampleInt);
        Debug.Log(item.exampleFloat);
        Debug.Log(item.exampleString);
        Debug.Log(item.exampleBool);
        var str = "exampleArray1: ";
        foreach(var value in item.exampleArray1)
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
