const net = require('net')
var protobuf = require("protobufjs")
var root = protobuf.loadSync("./proto/Example.proto")
var message = root.lookupType("Proto.Example")

const server = net.createServer((socket) => {
    socket.on('data', (buffer) => {
        // 收到客户端消息
        console.log(message.decode(buffer))
        // 响应一条数据测试
        var respond = message.create({ ExampleInt: -1, ExampleFloat: -2.5, ExampleString: 'cba', ExampleArray: [ { ItemDouble: 1.2, ItemBool: true } ] })
        socket.write(message.encode(respond).finish())
    })
})

server.listen(3000)