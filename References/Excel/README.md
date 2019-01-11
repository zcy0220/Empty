# 将xls转成protobuf供Unity使用的流程

### 使用事项
* Excels/: excel表格目录
    - excel 的前四行用于结构定义, 其余则为数据
    - 第一行：'-' 客户端和服务器都转 | 'ignore' 忽略列 | todo 区分客户端和服务器
    - 第二行：类型 支持 int float string bool array
    - 第三行：关键字
    - 第四行：注释
* 一键转表: Tools->Sheet->Protobuf
    