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
    private string mSheetName;
    /// <summary>
    /// 默认key为"id"
    /// </summary>
    private string mKey = "Id";
    /// <summary>
    /// 默认keyType为int
    /// </summary>
    private string mKeyType = "int";
    /// <summary>
    /// 默认只导出字典数据
    /// </summary>
    private EExportDataType mExportDataType = EExportDataType.ONLY_DICT;
    
    public SheetExportBase(string sheetName)
    {
        mSheetName = sheetName;
    }

    public SheetExportBase SetKey(string key)
    {
        mKey = key;
        return this;
    }

    public SheetExportBase SetExportDataType(EExportDataType exportDataType)
    {
        mExportDataType = exportDataType;
        return this;
    }

    public SheetExportBase SetKeyType(string keyType)
    {
        mKeyType = keyType;
        return this;
    }

    /// <summary>
    /// 导出自动生成脚本
    /// </summary>
    public string ExportScript()
    {
        var sb = new StringBuilder();
        sb.Append(SheetEditor.LineText(string.Format("//{0}", mSheetName), 1));
        string initFuncName = StringUtil.Concat("Init", mSheetName);
        string ListParamName = StringUtil.Concat(mSheetName, "List");
        string dictParamName = StringUtil.Concat("m" + mSheetName, "Dict");
        bool exportList = (mExportDataType == EExportDataType.ONLY_ARRAY || mExportDataType == EExportDataType.BOTH);
        bool exportDict = (mExportDataType == EExportDataType.ONLY_DICT || mExportDataType == EExportDataType.BOTH);

        // List
        if (exportList)
        {
            sb.Append(SheetEditor.LineText(string.Format("private List<{0}> m{1};", mSheetName, ListParamName), 1));
            sb.Append(SheetEditor.LineText(string.Format("public List<{0}> Get{1}()", mSheetName, ListParamName), 1));
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
            sb.Append(SheetEditor.LineText(string.Format("private Dictionary<{0}, Sheet.{1}> {2};", mKeyType, mSheetName, dictParamName), 1));
            sb.Append(SheetEditor.LineText(string.Format("public Sheet.{0} Get{0}({1} key)", mSheetName, mKeyType), 1));
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
            sb.Append(SheetEditor.LineText(string.Format("var items = GetSheetInfo<{0}List>(\"{0}\").Items;", mSheetName), 2));
            if (exportList)
            {
                sb.Append(SheetEditor.LineText(string.Format("m{0} = items;", ListParamName), 2));
            }
            if (exportDict)
            {
                sb.Append(SheetEditor.LineText(string.Format("{0} = new Dictionary<{1}, Sheet.{2}>();", dictParamName, mKeyType, mSheetName), 2));
                sb.Append(SheetEditor.LineText(string.Format("items.ForEach(item => {0}[item.{1}] = item);", dictParamName, mKey), 2));
            }
        }
        sb.Append(SheetEditor.LineText("}", 1));
        sb.Append(SheetEditor.LineText("", 1));

        return sb.ToString();
    }
}
