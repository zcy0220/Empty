# Lua脚本开发 (需要切换到[xlua]分支)
* [xLua](https://github.com/Tencent/xLua)
* 推荐lua开发工具：vscode + [luaide](https://www.showdoc.cc/luaide?page_id=687771476825747)

## 原理理解
* LuaCallCSharp：类、属性等。Lua端直接调用没用该标签导出的类时，会自动用反射访问。用该标签导出脚本，会生成wrap文件，当lua访问时，会先访问warp文件，warp文件再调用原文件代码，从而达到去反射效果，提高性能
* CSharpCallLua: 委托等。导出会在DelegatesGensBridge.cs中生成。直观一点：InvalidCastException: This type must add to CSharpCallLua：{funtion} 报该错误就在对应的方法上添加标签导出