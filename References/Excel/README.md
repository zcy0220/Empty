# 表格数据
* 2019.7.23：C#端增加了表格枚举功能，Lua端还没实现

## 转表
* Tools->Sheet->ExportBytes
* Tools->Sheet->ExportLua

## 使用事项
* excel必须是xlsx，必须关闭表格才能转表
* 示例项目用Bytes演示支持client数据，lua演示支持server数据用来测试客户端和服务器数据区分功能
* Excels/xlsx: excel表格目录
    - excel的前四行用于结构定义, 其余为数据
    - 第一行：'-' | 'ignore'(忽略该列)
    - 第二行：布尔(bool) 整型(int) 浮点数(float) 字符串(string) 数组(array<基本类型>) 枚举(xxxEnum, 自定义名字+Enum后缀)
    - 第三行：关键字(MoveSpeed, 首字母大写式驼峰命名规则)
    - 第四行：注释

## Excel示例
![excel](./images/001.png)

## Bytes数据
![bytes](./images/002.png)

## Lua数据
~~~lua
local Example = {
    [1]={exampleInt=1,exampleString="example",exampleFloat=1.5,exampleBool=false,exampleArray1={1},exampleArray2={1.5,2.5},exampleArray3={"a","b","c"},exampleServer="server1"},
    [2]={exampleInt=2,exampleString="-",exampleFloat=1.5,exampleBool=true,exampleArray1={1},exampleArray2={1.5,2.5},exampleArray3={"a","","c"},exampleServer="server2"},
}
return Example
~~~