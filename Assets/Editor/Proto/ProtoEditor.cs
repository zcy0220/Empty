/**
 * Proto导出工具
 */

using System.IO;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Text;
using Base.Debug;
using System.Collections.Generic;
using Editor.Proto;

public class ProtoEditor
{
    /// <summary>
    /// proto协议存放的路径
    /// </summary>
    private const string PROTOPATH = "Server/proto";
    /// <summary>
    /// Proto转CS的路径
    /// </summary>
    private static string OUTCSPATH = "Assets/Scripts/Net/Proto";
    /// <summary>
    /// 自动生成的NetMsg协议号和协议类型映射关系
    /// NetMsgConfig只是用来手动协议号和协议的配置文件
    /// </summary>
    private static string NETMSGCONFIG = "Server/NetMsgConfig.json";
    private static string OUTNETMSGPATH = "Assets/Scripts/Net/NetMsg.cs";

    /// <summary>
    /// Proto -> CS
    /// </summary>
    [MenuItem("Tools/Proto/ExportCS")]
    public static void ExportCS()
    {
        var protoDir = new DirectoryInfo(PROTOPATH);
        var files = protoDir.GetFiles();
        var fileSet = new HashSet<string>();
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
                sb.Append(SheetEditor.LineText("public class " + protocolStr + " {", 1));
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
                        var keys = line.Trim().Split(' ');
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
            var fileName = file.Name.Replace(file.Extension, "");
            fileSet.Add(fileName);
            // 写入
            Base.Utils.FileUtil.WriteAllText(OUTCSPATH + "/" + fileName + ".cs", sb.ToString());
        }
        // 删除没用的协议
        var csProtoDir = new DirectoryInfo(OUTCSPATH);
        var csFiles = csProtoDir.GetFiles();
        foreach(var file in csFiles)
        {
            var fileName = file.Name.Replace(file.Extension, "");
            if (!fileSet.Contains(fileName))
            {
                FileUtil.DeleteFileOrDirectory(file.FullName);
            }
        }
        AssetDatabase.Refresh();
        Debugger.Log("Proto ExportCS Done!");
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
            case "int64":
                return "long";
            default:
                return keyType;
        }
    }

    /// <summary>
    /// 自动生成NetMsg相关
    /// JsonUtility解析json的时候字段名要一致
    /// </summary>
    [MenuItem("Tools/Proto/ExportCSNetMsg")]
    private static void ExportCSNetMsg()
    {
        ExportCS();
        var config = Base.Utils.FileUtil.ReadAllText(NETMSGCONFIG);
        config = "{ \"list\": " + config + "}";
        var netMsg = UnityEngine.JsonUtility.FromJson<NetMsgConfig<Editor.Proto.NetMsg>>(config);

        // 生成NetMsg.cs
        var sb = new StringBuilder();
        sb.Append(SheetEditor.LineText("/**"));
        sb.Append(SheetEditor.LineText(" * Tool generation, do not modify!!!"));
        sb.Append(SheetEditor.LineText(" */\n"));
        sb.Append(SheetEditor.LineText("using System;\nusing Base.Debug;\nusing Base.Utils;\nusing System.Collections.Generic;\n"));
        sb.Append(SheetEditor.LineText("public class NetMsg\n{"));
        foreach (var msg in netMsg.list)
        {
            sb.Append(SheetEditor.LineText("public const int " + (msg.MsgName + "_msg").ToUpper() + " = " + msg.MsgId + ";", 1));
        }
        sb.Append(SheetEditor.LineText("//=============================================================================", 1));
        sb.Append(SheetEditor.LineText("private static Dictionary<int, Type> MsgIdTypeDict = new Dictionary<int, Type>();\n", 1));
        sb.Append(SheetEditor.LineText("public static void Init()", 1));
        sb.Append(SheetEditor.LineText("{", 1));
        foreach (var msg in netMsg.list)
        {
            var msgName = (msg.MsgName + "_msg").ToUpper();
            var str = "MsgIdTypeDict.Add({0}, typeof({1}));";
            sb.Append(SheetEditor.LineText(string.Format(str, msgName, msg.Response), 2));
        }
        sb.Append(SheetEditor.LineText("}\n", 1));
        sb.Append(SheetEditor.LineText("public static Type GetTypeByMsgId(int msgId)", 1));
        sb.Append(SheetEditor.LineText("{", 1));
        sb.Append(SheetEditor.LineText("if (MsgIdTypeDict.ContainsKey(msgId))", 2));
        sb.Append(SheetEditor.LineText("{", 2));
        sb.Append(SheetEditor.LineText("return MsgIdTypeDict[msgId];", 3));
        sb.Append(SheetEditor.LineText("}", 2));
        sb.Append(SheetEditor.LineText("Debugger.LogError(StringUtil.Concat(\"Not Find Msg Type! Error MsgId: \", msgId));", 2));
        sb.Append(SheetEditor.LineText("return null;", 2));
        sb.Append(SheetEditor.LineText("}", 1));
        // 写入
        sb.Append(SheetEditor.LineText("}"));
        Base.Utils.FileUtil.WriteAllText(OUTNETMSGPATH, sb.ToString());
        Debugger.Log("Proto ExportNetMsg Done!");
    }

    //[MenuItem("Tools/Proto/ExportLua")]
    //public static void ExportLua()
    //{
    //}
}
