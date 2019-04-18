/**
 * SheetManager的拓展功能
 */

using UnityEngine;
using Base.Common;
using Base.Utils;

public partial class SheetManager : Singleton<SheetManager>
{
    public const string PREFIX = "Sheets/";
    public const string POSTFIX = ".bytes";
    /// <summary>
    /// 获得表格数据
    /// </summary>
    public T GetSheetInfo<T>(string fileName)
    {
        var text = ResourceManager.Instance.SyncLoad<TextAsset>(StringUtil.Concat(PREFIX, fileName, POSTFIX));
        return ProtobufUtil.NDeserialize<T>(text.bytes);
    }
}
