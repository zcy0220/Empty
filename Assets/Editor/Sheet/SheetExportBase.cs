/**
 * 配置表导出格式基类
 */

using System.Text;
using Base.Utils;

public enum EExportDataType
{
    NONE = 0,        // 什么数据都不导出
    ONLY_DICT = 1,   // 只导出字典
    ONLY_ARRAY = 2,  // 只导出数组
    BOTH = 3         // 同时导出数组和字典
}

public class SheetExportBase
{
    private string sheetName;
    /// <summary>
    /// 默认key为"id"
    /// </summary>
    private string key = "id";
    /// <summary>
    /// 默认keyType为int
    /// </summary>
    private string keyType = "int";
    /// <summary>
    /// 默认只导出字典数据
    /// </summary>
    private EExportDataType exportDataType = EExportDataType.ONLY_DICT;
    
    public SheetExportBase(string sheetName)
    {
        this.sheetName = sheetName;
    }

    public SheetExportBase SetKey(string key)
    {
        this.key = key;
        return this;
    }

    public SheetExportBase SetExportDataType(EExportDataType exportDataType)
    {
        this.exportDataType = exportDataType;
        return this;
    }

    public SheetExportBase SetKeyType(string keyType)
    {
        this.keyType = keyType;
        return this;
    }

    /// <summary>
    /// 导出自动生成脚本
    /// </summary>
    public string ExportScript()
    {
        var sb = new StringBuilder();
        sb.Append(SheetEditor.LineText(string.Format("//{0}", sheetName), 1));
        string initFuncName = StringUtil.Concat("Init", sheetName);
        string ListParamName = StringUtil.Concat(sheetName, "List");
        string dictParamName = StringUtil.Concat("m" + sheetName, "Dict");
        bool exportList = (exportDataType == EExportDataType.ONLY_ARRAY || exportDataType == EExportDataType.BOTH);
        bool exportDict = (exportDataType == EExportDataType.ONLY_DICT || exportDataType == EExportDataType.BOTH);

        // List
        if (exportList)
        {
            sb.Append(SheetEditor.LineText(string.Format("private List<{0}> m{1};", sheetName, ListParamName), 1));
            sb.Append(SheetEditor.LineText(string.Format("public List<{0}> Get{1}()", sheetName, ListParamName), 1));
            sb.Append(SheetEditor.LineText("{", 1));
            sb.Append(SheetEditor.LineText(string.Format("if (m{0} == null)", ListParamName), 2));
            sb.Append(SheetEditor.LineText("{", 2));
            sb.Append(SheetEditor.LineText(string.Format("{0}();", initFuncName), 3));
            sb.Append(SheetEditor.LineText("}", 2));
            sb.Append(SheetEditor.LineText(string.Format("return m{0};", ListParamName), 2));
            sb.Append(SheetEditor.LineText("}", 1));
        }

        // Dictionary
        if (exportDict)
        {
            sb.Append(SheetEditor.LineText(string.Format("private Dictionary<{0}, {1}> {2};", keyType, sheetName, dictParamName), 1));
            sb.Append(SheetEditor.LineText(string.Format("public {0} Get{0}({1} key)", sheetName, keyType), 1));
            sb.Append(SheetEditor.LineText("{", 1));
            sb.Append(SheetEditor.LineText(string.Format("if ({0} == null)", dictParamName), 2));
            sb.Append(SheetEditor.LineText("{", 2));
            sb.Append(SheetEditor.LineText(string.Format("{0}();", initFuncName), 3));
            sb.Append(SheetEditor.LineText("}", 2));
            sb.Append(SheetEditor.LineText(string.Format("return {0}[key];", dictParamName), 2));
            sb.Append(SheetEditor.LineText("}", 1));
        }

        // 初始化方法
        sb.Append(SheetEditor.LineText(string.Format("private void {0}()", initFuncName), 1));
        sb.Append(SheetEditor.LineText("{", 1));
        if (exportList || exportDict)
        {
            sb.Append(SheetEditor.LineText(string.Format("var items = GetSheetInfo<{0}List>(\"{0}\").Items;", sheetName), 2));
            if (exportList)
            {
                sb.Append(SheetEditor.LineText(string.Format("m{0} = items;", ListParamName), 2));
            }
            if (exportDict)
            {
                sb.Append(SheetEditor.LineText(string.Format("{0} = new Dictionary<{1}, {2}>();", dictParamName, keyType, sheetName), 2));
                sb.Append(SheetEditor.LineText(string.Format("items.ForEach(item => {0}[item.{1}] = item);", dictParamName, key), 2));
            }
        }
        sb.Append(SheetEditor.LineText("}", 1));
        sb.Append(SheetEditor.LineText("", 1));

        return sb.ToString();
    }
}
