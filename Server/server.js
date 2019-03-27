const net = require('net')
var protobuf = require("protobufjs")
var root = protobuf.loadSync("./proto/Example.proto")
var message = root.lookupType("Proto.Example")

const server = net.createServer((socket) => {
    socket.on('data', (buffer) => {
        // 收到客户端消息
        console.log(message.decode(buffer))
        // 响应一条数据测试
        var respond = message.create({ ExampleInt: -1, ExampleFloat: -2.5, ExampleString: 'cba' })
        // socket.write(message.encode(respond).finish())
        var errMsg = message.verify(respond)
        if (errMsg) throw Error(errMsg)
        var buffer = new Buffer("Hell")
        socket.write(buffer)
    })
})

server.listen(3000)