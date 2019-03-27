# 本地服务器
* [Node.js参考](https://github.com/wugui0220/BLOG)

## 环境准备
- node
- yarn(或npm)
```sh
yarn add protobufjs
```
## Server/server.js
``` js
const net = require('net')
var protobuf = require("protobufjs")
var root = protobuf.loadSync("./proto/Example.proto")
var message = root.lookupType("Proto.Example")

const server = net.createServer((socket) => {
    socket.on('data', (buffer) => {
        console.log(message.decode(buffer))
    })
})

server.listen(3000)
```

## 执行脚本
```sh
node server.js
```
## 服务器接受到的数据
![result](./images/002.png)