# 表格数据

## 转表
* Tools->Sheet->ExportBytes

## 使用事项
* excel必须是xlsx，必须关闭表格才能转表
* Excels/xlsx: excel表格目录
    - excel的前四行用于结构定义, 其余为数据
    - 第一行：'-' | 'ignore'(忽略该列)
    - 第二行：布尔(bool) 整型(int) 浮点数(float) 字符串(string) 数组(array<基本类型>) 枚举(xxxEnum, 自定义名字+Enum后缀)
    - 第三行：关键字(MoveSpeed, 首字母大写式驼峰命名规则)
    - 第四行：注释