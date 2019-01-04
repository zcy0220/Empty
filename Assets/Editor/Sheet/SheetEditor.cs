/**
 * 转表编辑器
 */

using System.Collections;
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

    [MenuItem("Tools/Sheet/Protobuf")]
    private static void Sheet2Protobuf()
    {
        var sheetDir = new DirectoryInfo(SHEETROOTPATH);
        var files = sheetDir.GetFiles();
        var sheetNames = new List<string>();
        //生成CS
        var sheetCSSB = new StringBuilder();
        sheetCSSB.Append(LineText("/**"));
        sheetCSSB.Append(LineText(" * Tool generation, do not modify!!!"));
        sheetCSSB.Append(LineText(" */\n"));
        sheetCSSB.Append(LineText("using ProtoBuf;\nusing System.Collections.Generic;\n"));
        sheetCSSB.Append(LineText("namespace Sheet"));
        sheetCSSB.Append(LineText("{"));
        for (var i = 0; i < files.Length; i++)
        {
            var file = files[i];
            var name = file.Name.Replace(SHEETEXT, "");
            sheetNames.Add(name);
            sheetCSSB.Append(LineText("[ProtoContract]", 1));
            sheetCSSB.Append(LineText("public class " + name, 1));
            sheetCSSB.Append(LineText("{", 1));
            var stream = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            var dataSet = excelReader.AsDataSet();
            var table = dataSet.Tables[0];
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
                        sheetCSSB.Append(LineText(string.Format("public List<{0}> {1};", match.Groups[1].Value, row2), 2));
                    }
                    else
                    {
                        sheetCSSB.Append(LineText(string.Format("public {0} {1};", row1, row2), 2));
                    }
                }
            }
            sheetCSSB.Append(LineText("}", 1));
        }
        sheetCSSB.Append(LineText("}"));
        Base.Utils.FileUtil.WriteAllText("Assets/Scripts/Sheet/SheetProtobuf.cs", sheetCSSB.ToString());
        Debugger.Log("Generate Protobuf CS Done");

        AssetDatabase.Refresh();
        //生成bytes
        //var provider = new CSharpCodeProvider();
        //var parameters = new CompilerParameters();
        //var result = provider.CompileAssemblyFromSource(parameters, sourceFile);
        //if (result.Errors.Count == 0)
        //{
        //    Debug.Log("编译成功");
        //    var ass = result.CompiledAssembly;
        //    var obj = ass.CreateInstance("MyCode1");
        //    var method = obj.GetType().GetMethod("Test");
        //    var sum = method.Invoke(obj, new object[] { 1, 5 });
        //    Debug.Log(sum);
        //    var value = obj.GetType().GetField("a").GetValue(obj);
        //    Debug.Log(value);
        //}
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
}
