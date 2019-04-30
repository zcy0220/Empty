/**
 * 根据Prefab导出UILua
 */

using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using UnityEngine.UI;
using System.Collections.Generic;

public class UILuaEditor
{
    /// <summary>
    /// 脚本路径
    /// </summary>
    private static string LuaPrefix = "Assets/LuaScripts/Views/";
    /// <summary>
    /// 不合法命名列表
    /// </summary>
    private static List<string> IllegalNameList = new List<string>();
    /// <summary>
    /// 按钮命名列表
    /// </summary>
    private static List<string> BtnNameList = new List<string>();
    /// <summary>
    /// UI类型和其前缀的对应关系
    /// 检测最常见的UI控件，顺序有关，因为关系到同一控件有2个以上下列组件时的命名
    /// </summary>
    private static Dictionary<Type, string> TypePrefixDict = new Dictionary<Type, string>
    {
        { typeof(InputField), "Input"},
        { typeof(ScrollRect), "Scroll"},
        { typeof(Slider), "Slider"},
        { typeof(Button), "Btn"},
        { typeof(Image), "Img"},
        { typeof(Text), "Text"},
    };

    /// <summary>
    /// 选中Prefab右键导出Lua脚本
    /// Prefab的路径会对应到Lua脚本路径下
    /// GameAssets/Views/Login/ViewLogin.prefab -> LuaScripts/Views/Login/ViewLogin.lua
    /// </summary>
    [MenuItem("Assets/ExportUILua")]
    private static void DoExportUILua()
    {
        IllegalNameList.Clear();
        BtnNameList.Clear();
        var select = Selection.activeObject;
        var path = AssetDatabase.GetAssetPath(select);
        var index0 = path.LastIndexOf(".");
        var index1 = path.LastIndexOf("/");
        var index2 = path.LastIndexOf("/", index1 - 1);
        var fileName = path.Substring(index1 + 1, index0 - index1 - 1);
        Debug.Log(fileName);
        var luaPath = LuaPrefix + path.Substring(index2 + 1, index0 - index2 - 1) + ".lua";
        Debug.Log(luaPath);

        // 检测命名
        var transform = Selection.activeGameObject.transform;
        CheckNames(transform);
        if (IllegalNameList.Count > 0)
        {
            foreach (var illegalName in IllegalNameList)
            {
                Debug.LogError(illegalName);
            }
            return;
        }
        if (File.Exists(luaPath))
        {
            Debug.Log("Lua 已经存在");
        }
        else
        {
            var sb = new StringBuilder();
            sb.Append(SheetEditor.LineText("--[[", 0));
            sb.Append(SheetEditor.LineText("@desc:" + fileName, 1));
            sb.Append(SheetEditor.LineText("]]\n", 0));
            sb.Append(SheetEditor.LineText(string.Format("local {0} = Class(\"{0}\", ViewBase)\n", fileName), 0));
            sb.Append(SheetEditor.LineText("-- 构造 数据初始化", 0));
            sb.Append(SheetEditor.LineText("local function Ctor(self, gameObject)", 0));
            sb.Append(SheetEditor.LineText(string.Format("{0}.super.Ctor(self, gameObject)", fileName), 1));
            sb.Append(SheetEditor.LineText("end\n", 0));
            sb.Append(SheetEditor.LineText("-- 创建 UI控件初始化", 0));
            sb.Append(SheetEditor.LineText("local function Create(self)", 0));
            sb.Append(SheetEditor.LineText(string.Format("{0}.super.Create(self)", fileName), 1));
            sb.Append(SheetEditor.LineText("end\n", 0));
            sb.Append(SheetEditor.LineText("-- 关闭", 0));
            sb.Append(SheetEditor.LineText("local function Close(self)", 0));
            sb.Append(SheetEditor.LineText(string.Format("{0}.super.Close(self)", fileName), 1));
            sb.Append(SheetEditor.LineText("end\n", 0));

            // 绑定按钮的监听事件
            foreach (var btnName in BtnNameList)
            {
                sb.Append(SheetEditor.LineText(string.Format("-- {0}的监听事件", btnName), 0));
                sb.Append(SheetEditor.LineText(string.Format("local function On{0}(self)", btnName), 0));
                sb.Append(SheetEditor.LineText("end\n", 0));
            }

            sb.Append(SheetEditor.LineText(string.Format("{0}.Ctor = Ctor", fileName), 0));
            sb.Append(SheetEditor.LineText(string.Format("{0}.Create = Create", fileName), 0));
            sb.Append(SheetEditor.LineText(string.Format("{0}.Close = Close", fileName), 0));

            // 绑定按钮的监听事件
            foreach (var btnName in BtnNameList)
            {
                sb.Append(SheetEditor.LineText(string.Format("{0}.On{1} = On{1}", fileName, btnName), 0));
            }

            sb.Append(SheetEditor.LineText(string.Format("\nreturn {0}", fileName), 0));
            Base.Utils.FileUtil.WriteAllText(luaPath, sb.ToString());
            AssetDatabase.Refresh();
            Debug.Log("Success!");
        }
    }

    /// <summary>
    /// 检测命名规范
    /// </summary>
    private static void CheckNames(Transform transform, string parentName = "")
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var childName = child.gameObject.name;
            var fullChildName = parentName == "" ? childName : parentName + "/" + childName;
            foreach (var type in TypePrefixDict.Keys)
            {
                var component = child.GetComponent(type);
                if (component != null)
                {
                    // 如果有该组件，但命名又不是以上规范去命名的，就报错
                    var prefix = TypePrefixDict[type];
                    if (!childName.StartsWith(prefix))
                    {
                        IllegalNameList.Add(fullChildName);
                    }
                    else
                    {
                        if (type == typeof(Button)) BtnNameList.Add(childName);
                    }
                    break;
                }
            }
            CheckNames(child, fullChildName);
        }
    }
}

