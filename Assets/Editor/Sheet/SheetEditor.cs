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
        sheetCSSB.Append(LineText("//Tool generation, do not modify !!!!"));
        for (var i = 0; i < files.Length; i++)
        {
            var file = files[i];
            sheetNames.Add(file.Name.Replace(SHEETEXT, ""));
            var stream = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            var dataSet = excelReader.AsDataSet();
            var table = dataSet.Tables[0];
            var cols = table.Columns.Count;
            for(var j = 0; j < cols; j++)
            {
                var row0 = table.Rows[0][j].ToString();
                if (!row0.Equals("ignore"))
                {
                    var row1 = table.Rows[1][j].ToString();
                    var row2 = table.Rows[2][j].ToString();
                    //sb.Append(LineText("using Sheet;", 0));
                    //sb.Append(LineText("using System.Collections.Generic;", 0));
                    //sb.Append(LineText("using Base.Common;", 0));
                    //sb.Append(LineText("", 0));
                    //sb.Append(LineText("public partial class SheetManager : Singleton<SheetManager>", 0));
                    //sb.Append(LineText("{", 0));
                }
            }
        }
        //FileUtil.SafeWriteAllText("Assets/Scripts/Sheet/SheetManager.cs", sb.ToString());
        //Debugger.Log("Generate SheetManager CS Done");

        //AssetDatabase.Refresh()
        //生成bytes
    }

    /// <summary>
    /// 获得每一行的字符串
    /// </summary>
    private static string LineText(string text, int tabCount = 0)
    {
        string ret = "";
        for (int i = 1; i <= tabCount; i++)
        {
            ret += "\t";
        }
        ret += (text + "\n");
        return ret;
    }
}
