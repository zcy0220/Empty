/**
 * Proto导出工具
 */

using System.IO;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Text;

public class ProtoEditor
{
    private const string PROTOPATH = "Server/proto";
    private static string OUTCSPATH = "Assets/Scripts/Net/Proto/{0}.cs";

    [MenuItem("Tools/Proto/ExportCS")]
    public static void ExportCS()
    {
        var protoDir = new DirectoryInfo(PROTOPATH);
        var files = protoDir.GetFiles();
        foreach (var file in files)
        {
            var sb = new StringBuilder();
            sb.Append(SheetEditor.LineText("/**"));
            sb.Append(SheetEditor.LineText(" * Tool generation, do not modify!!!"));
            sb.Append(SheetEditor.LineText(string.Format(" * Generated from: {0}", file.Name)));
            sb.Append(SheetEditor.LineText(" */\n"));
            sb.Append(SheetEditor.LineText("using ProtoBuf;\nusing System.Collections.Generic;\n"));

            // 读取proto文件
            var text = Base.Utils.FileUtil.ReadAllText(file.FullName);
            // 分离出命名空间
            var nameSpace = Regex.Match(text, "(?<=package ).*(?=;)").Groups[0].Value;
            sb.Append(SheetEditor.LineText("namespace " + nameSpace + " {\n"));

            // 分离出协议名
            var protocols = Regex.Matches(text, "(?<=message ).*(?=\r*\n*{)");
            foreach (var protocol in protocols)
            {
                var protocolStr = protocol.ToString().Replace("\r", "");
                protocolStr = protocolStr.Replace("\n", "");
                sb.Append(SheetEditor.LineText("[ProtoContract]", 1));
                sb.Append(SheetEditor.LineText("public partial class " + protocolStr + " {", 1));
                var regex = "(?<=message " + protocolStr + "\r*\n*{)[^}]*(?=})";
                var keyStr = Regex.Match(text, regex).Groups[0].Value;
                // 分离协议字段
                keyStr = keyStr.Replace("\r", "").Replace("\n", "").Replace("\t", "");
                var keyLines= keyStr.Split(';');
                var count = 0;
                foreach (var line in keyLines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        count++;
                        var keys = line.Split(' ');
                        var keyCode = keys[0];
                        var keyType = keys[1];
                        var keyValue = keys[2];
                        sb.Append(SheetEditor.LineText(string.Format("[ProtoMember({0})]", count), 2));
                        if (keyCode.Equals("repeated"))
                        {
                            sb.Append(SheetEditor.LineText(string.Format("public List<{0}> {1};", ConvertType(keyType), keyValue), 2));
                        }
                        else
                        {
                            sb.Append(SheetEditor.LineText(string.Format("public {0} {1};", ConvertType(keyType), keyValue), 2));
                        }
                    }
                }
                sb.Append(SheetEditor.LineText("}\n", 1));
            }

            sb.Append(SheetEditor.LineText("}"));
            // 写入
            Base.Utils.FileUtil.WriteAllText(string.Format(OUTCSPATH, file.Name.Replace(file.Extension, "")), sb.ToString());
        }

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 转换关键字类型
    /// proto: int32 -> CS: int
    /// </summary>
    public static string ConvertType(string keyType)
    {
        switch(keyType)
        {
            case "int32":
                return "int";
            default:
                return keyType;
        }
    }

    //[MenuItem("Tools/Proto/ExportLua")]
    //public static void ExportLua()
    //{
    //}
}
