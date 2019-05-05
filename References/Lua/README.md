# Lua脚本开发 (需要切换到[xlua]分支)
* [xLua](https://github.com/Tencent/xLua)
* 推荐lua开发工具：vscode + [luaide](https://www.showdoc.cc/luaide?page_id=687771476825747)
* Lua脚本目录：Assets/LuaScripts。AB打包时会在GameAssets/LuaScripts下生成*.lua.bytes供真机使用
* [lua快速开发UI](UILUA.md)

## 原理理解
* LuaCallCSharp：类、属性等。Lua端直接调用没用该标签导出的类时，会自动用反射访问。用该标签导出脚本，会生成wrap文件，当lua访问时，会先访问warp文件，warp文件再调用原文件代码，从而达到去反射效果，提高性能
* CSharpCallLua: 委托等。导出会在DelegatesGensBridge.cs中生成。直观一点：InvalidCastException: This type must add to CSharpCallLua：{funtion} 报该错误就在对应的方法上添加标签导出

## Lua基本框架
```sh
└── LuaScripts
    ├── Base                  # 基础公用模块(基本上就是C#端翻成Lua)
    │   ├── Collections       # 常用的集合（Stack, Queue）
    │   ├── Common            # 公共方法（Class, Singleton等）
    │   ├── Debug             # 调试器
    │   ├── Event             # 事件管理
    │   ├── Extension         # Lua端系统方法拓展（string的拓展等）
    │   ├── Pool              # 对象池
    │   ├── Timer             # 定时器
    │   ├── Update            # Update管理
    │   └── Init              # 基础公共模块初始化
    ├── Global                # 全局内容
    │   └── Init              # 全局内容初始化
    ├── Modules               # 模块目录
    │   └── ...               # 各个模块
    ├── Views                 # 视图UI目录
    │   ├── ...               # 各个功能UI
    │   ├── ViewConfig        # UI与Prefab对应配置
    │   └── ViewManager       # 视图UI管理
    ├── Config                # 全局配置文件
    └── GameMain              # Lua程序入口
```