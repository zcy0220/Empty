/**
 * 转表编辑器
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Excel;
using System.Data;
using System;
using System.Text;
using Base.Utils;
using Base.Debug;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

public class SheetEditor
{
    private const string SHEETROOTPATH = "Excels";
    private const string SHEETEXT = ".xlsx";
    private static string OUTSHEETCSPATH = "Assets/Scripts/Sheet/SheetProtobuf.cs";
    private static string OUTSHEETMANAGERPATH = "Assets/Scripts/Sheet/SheetManager.cs";
    private static string OUTSHEETBYTES = "Assets/Resources/Sheets/{0}.bytes";

    /// <summary>
    /// 默认情况下，表格只需导出以id为key的字典。特殊情况下，要在下面自定义key和导出数据的类型
    /// </summary>
    private static readonly Dictionary<string, SheetExportBase> sheetExportConst = new Dictionary<string, SheetExportBase>
    {
        { "Example", new SheetExportBase("Example").SetKey("exampleInt").SetExportDataType(EExportDataType.BOTH)}
    };

    [MenuItem("Tools/Sheet/Protobuf")]
    private static void Sheet2Protobuf()
    {
        var sheetDir = new DirectoryInfo(SHEETROOTPATH);
        var files = sheetDir.GetFiles();
        var sheetDict = new Dictionary<string, DataTable>();

        //------------------------------生成CS--------------------------------
        var sheetCSSB = new StringBuilder();
        sheetCSSB.Append(LineText("/**"));
        sheetCSSB.Append(LineText(" * Tool generation, do not modify!!!"));
        sheetCSSB.Append(LineText(" */\n"));
        sheetCSSB.Append(LineText("using ProtoBuf;\nusing System.IO;\nusing System.Collections.Generic;\n"));
        sheetCSSB.Append(LineText("namespace Sheet"));
        sheetCSSB.Append(LineText("{"));
        sheetCSSB.Append(LineText(GetBaseArrayCode()));
        for (var i = 0; i < files.Length; i++)
        {
            var file = files[i];
            if (file.Extension != SHEETEXT) continue;
            var name = file.Name.Replace(SHEETEXT, "");
            sheetCSSB.Append(LineText("[ProtoContract]", 1));
            sheetCSSB.Append(LineText("public class " + name, 1));
            sheetCSSB.Append(LineText("{", 1));
            var stream = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            var dataSet = excelReader.AsDataSet();
            var table = dataSet.Tables[0];
            sheetDict.Add(name, table);
            var cols = table.Columns.Count;
            var count = 0;
            for(var j = 0; j < cols; j++)
            {
                var row0 = table.Rows[0][j].ToString();
                if (!row0.Equals("ignore"))
                {
                    count++;
                    var row1 = table.Rows[1][j].ToString();
                    var row2 = table.Rows[2][j].ToString();
                    sheetCSSB.Append(LineText(string.Format("[ProtoMember({0})]", count), 2));
                    if (row1.StartsWith("array"))
                    {
                        var reg = new Regex("<(.*)>");
                        var match = reg.Match(row1);
                        var value = match.Groups[1].Value;
                        sheetCSSB.Append(LineText(string.Format("public List<{0}> {1} = new List<{0}>();", value, row2), 2));
                    }
                    else
                    {
                        sheetCSSB.Append(LineText(string.Format("public {0} {1};", row1, row2), 2));
                    }
                }
            }
            sheetCSSB.Append(LineText("}\n", 1));
            sheetCSSB.Append(LineText("[ProtoContract]", 1));
            sheetCSSB.Append(LineText("public class " + name + "List : BaseList", 1));
            sheetCSSB.Append(LineText("{", 1));
            sheetCSSB.Append(LineText("[ProtoMember(1)]", 2));
            sheetCSSB.Append(LineText(string.Format("public List<{0}> Items = new List<{0}>();", name), 2));
            sheetCSSB.Append(LineText("}\n", 1));
        }
        sheetCSSB.Append(LineText("}"));
        Base.Utils.FileUtil.WriteAllText(OUTSHEETCSPATH, sheetCSSB.ToString());
        Debugger.Log("Generate Protobuf CS Done!");

        //------------------------------生成SheetManager------------------------------
        var sheetManagerSB = new StringBuilder();
        sheetManagerSB.Append(LineText("/**"));
        sheetManagerSB.Append(LineText(" * Tool generation, do not modify!!!"));
        sheetManagerSB.Append(LineText(" */\n"));
        sheetManagerSB.Append(LineText("using Sheet;\nusing Base.Common;\nusing System.Collections.Generic;\n"));
        sheetManagerSB.Append(LineText("public partial class SheetManager : Singleton<SheetManager>"));
        sheetManagerSB.Append(LineText("{"));

        foreach (var table in sheetDict)
        {
            var sheetName = table.Key;
            SheetExportBase sheetExportBase;
            if (!sheetExportConst.TryGetValue(sheetName, out sheetExportBase))
            {
                sheetExportBase = new SheetExportBase(sheetName);
            }

            string exportText = sheetExportBase.ExportScript();
            sheetManagerSB.Append(exportText);
        }

        sheetManagerSB.Append("}");
        Base.Utils.FileUtil.WriteAllText(OUTSHEETMANAGERPATH, sheetManagerSB.ToString());
        Debugger.Log("Generate SheetManager Done!");

        //------------------------------生成bytes-------------------------------------
        var provider = new CSharpCodeProvider();
        var parameters = new CompilerParameters();
        parameters.ReferencedAssemblies.Add(Application.dataPath + "/Plugins/Protobuf/protobuf-net.dll");
#if UNITY_EDITOR_OSX
        var pathName = "PATH";
        var envPath = Environment.GetEnvironmentVariable(pathName);
        var monoPath = Path.Combine(EditorApplication.applicationContentsPath, "Mono/bin");
        Environment.SetEnvironmentVariable(pathName, envPath + ":" + monoPath, EnvironmentVariableTarget.Process);
#endif
        var result = provider.CompileAssemblyFromSource(parameters, sheetCSSB.ToString());
        if (result.Errors.Count == 0)
        {
            Debugger.Log("SheetProtobuf Build Success!");
            var ass = result.CompiledAssembly;
            foreach(var table in sheetDict)
            {
                var name = table.Key;
                var data = table.Value;
                var listObj = ass.CreateInstance("Sheet." + name + "List");
                var list = listObj.GetType().GetField("Items").GetValue(listObj);
                var addMethod = list.GetType().GetMethod("Add");
                var rows = data.Rows.Count;
                var cols = data.Columns.Count;
                for (var i = 4; i < rows; i++)
                {
                    var obj = ass.CreateInstance("Sheet." + name);
                    var type = obj.GetType();
                    for (var j = 0; j < cols; j++)
                    {
                        var row1 = data.Rows[1][j].ToString();
                        var row2 = data.Rows[2][j].ToString();
                        var value = data.Rows[i][j];
                        if (row1.StartsWith("array"))
                        {
                            var reg = new Regex("<(.*)>");
                            var match = reg.Match(row1);
                            var valueType = match.Groups[1].Value;
                            var arrayValue = obj.GetType().GetField(row2).GetValue(obj);
                            var valueStr = value.ToString();
                            valueStr = valueStr.Substring(1, valueStr.Length - 2);
                            var valueList = valueStr.Split(',');
                            for (var k = 0; k < valueList.Length; k++)
                            {
                                switch (valueType)
                                {
                                    case "int":
                                        ((List<int>)arrayValue).Add(Convert.ToInt32(valueList[k]));
                                        break;
                                    case "float":
                                        ((List<float>)arrayValue).Add(Convert.ToSingle(valueList[k]));
                                        break;
                                    case "string":
                                        ((List<string>)arrayValue).Add(valueList[k]);
                                        break;
                                    case "bool":
                                        ((List<bool>)arrayValue).Add(Convert.ToBoolean(valueList[k]));
                                        break;
                                }
                            }
                        }
                        else
                        {
                            SetValue(type, row1, row2, obj, value);
                        }  
                    }

                    addMethod.Invoke(list, new object[] { obj });
                }
                var exportMethod = listObj.GetType().GetMethod("Export");
                var outFile = string.Format(OUTSHEETBYTES, name);
                if (Base.Utils.FileUtil.CheckFileAndCreateDirWhenNeeded(outFile))
                {
                    exportMethod.Invoke(listObj, new object[] { outFile });
                }
            }
            Debugger.Log("Generate Bytes Done!");
        }
        else
        {
            Debugger.LogError("SheetProtobuf Build Failed!");
        }
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Sets the value.
    /// </summary>
    private static void SetValue(Type type, string key1, string key2, object obj, object value)
    {
        switch(key1)
        {
            case "int":
                type.GetField(key2).SetValue(obj, Convert.ToInt32(value));
                break;
            case "float":
                type.GetField(key2).SetValue(obj, Convert.ToSingle(value));
                break;
            case "string":
                type.GetField(key2).SetValue(obj, Convert.ToString(value));
                break;
            case "bool":
                type.GetField(key2).SetValue(obj, Convert.ToBoolean(value));
                break;
        }
    }

    /// <summary>
    /// 获得每一行的字符串
    /// </summary>
    public static string LineText(string text, int tabCount = 0)
    {
        string ret = "";
        for (int i = 1; i <= tabCount; i++)
        {
            ret = StringUtil.Concat("\t", ret);
        }
        ret = StringUtil.Concat(ret, text);
        ret = StringUtil.Concat(ret, "\n");
        return ret;
    }

    /// <summary>
    /// Gets the base array code.
    /// </summary>
    private static string GetBaseArrayCode()
    {
        return @"    public class BaseList
    {
        public void Export(string outFile)
        {
            using (MemoryStream m = new MemoryStream())
            {
                Serializer.Serialize(m, this);
                m.Position = 0;
                int length = (int)m.Length;
                var buffer = new byte[length];
                m.Read(buffer, 0, length);
                File.WriteAllBytes(outFile, buffer);
            }
        }
    }
    ";
    }
}
