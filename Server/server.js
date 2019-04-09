const net = require('net')
const protobuf = require("protobufjs")
const root = protobuf.loadSync("./proto/User.proto")
const request = root.lookupType("User.LoginRequest")
const response = root.lookupType("User.LoginResponse")

/**
 * 因为服务器只是用来测试，所以并没有做粘包分包的处理
 * todo: 根据长度处理粘包分包，根据协议号处理对应协议内容
 */
const server = net.createServer((socket) => {
    socket.on('data', (buffer) => {
        var length = buffer.readInt32BE(0);
        console.log("Length: " + length);
        var msgId = buffer.readInt32BE(4);
        console.log("MsgId: " + msgId);
        var data = buffer.slice(8, buffer.length);
        console.log(request.decode(data));
        //-------------------------------------------------
        var respond = response.create({ ExampleInt: -1, ExampleFloat: -2.5, ExampleString: 'cba', ExampleArray: [ { ItemDouble: 1.2, ItemBool: true } ] })
        socket.write(message.encode(respond).finish())
    })
})

server.listen(3000)