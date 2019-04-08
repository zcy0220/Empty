const net = require('net')
const protobuf = require("protobufjs")
const root = protobuf.loadSync("./proto/User.proto")
const message = root.lookupType("User.LoginRequest")

const server = net.createServer((socket) => {
    socket.on('data', (buffer) => {
        // // 收到客户端消息
        // console.log(message.decode(buffer))
        // // 响应一条数据测试
        // var respond = message.create({ ExampleInt: -1, ExampleFloat: -2.5, ExampleString: 'cba', ExampleArray: [ { ItemDouble: 1.2, ItemBool: true } ] })
        // socket.write(message.encode(respond).finish())
        console.log(buffer.toString())
        socket.write(new Buffer("Back " + buffer.toString()))
    })
})

server.listen(3000)