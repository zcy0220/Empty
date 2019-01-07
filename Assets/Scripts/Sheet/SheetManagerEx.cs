/**
 * SheetManager的拓展功能，手动添加
 */

using ProtoBuf;
using UnityEngine;
using System.IO;
using Base.Common;
using Base.Utils;

public partial class SheetManager : Singleton<SheetManager>
{
    /// <summary>
    /// 获得表格数据
    /// </summary>
    public T GetSheetInfo<T>(string fileName)
    {
        //读取表格数据要根据具体项目而定
        var text = Resources.Load<TextAsset>("Sheets/" + fileName);
        return ProtobufUtil.NDeserialize<T>(text.bytes);
    }
}
