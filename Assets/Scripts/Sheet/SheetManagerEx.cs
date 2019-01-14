/**
 * SheetManager的拓展功能
 */

using UnityEngine;
using Base.Common;
using Base.Utils;

public partial class SheetManager : Singleton<SheetManager>
{
    const string SHEETPATH = "Sheets/";

    /// <summary>
    /// 获得表格数据
    /// </summary>
    public T GetSheetInfo<T>(string fileName)
    {
        var text = ResourceManager.Instance.LoadSheet(StringUtil.Concat(SHEETPATH, fileName));
        return ProtobufUtil.NDeserialize<T>(text.bytes);
    }
}
