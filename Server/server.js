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
        //------------------------接收客户端数据测试--------------------------
        console.log(buffer)
        var length = buffer.readInt32BE(0);
        console.log("Length: " + length);
        var msgId = buffer.readInt32BE(4);
        console.log("MsgId: " + msgId);
        var data = buffer.slice(8, buffer.length);
        console.log(request.decode(data));
        //------------------------发送服务器数据测试--------------------------
        var respondData = {
            Result: 0,
            User: {
                Base: { UID: 123, Name: "zcy" },
                Items: [ { Id: 1, Num: 100 }, { Id: 2, Num: 200 } ]
            }
        }
        var respond = response.create(respondData)
        var msgBuffer = response.encode(respond).finish()
        var size = 4 + msgBuffer.length
        var preBuffer = Buffer.alloc(8)
        preBuffer.writeInt32BE(size)
        preBuffer.writeInt32BE(msgId, 4)
        var sendBuffer = Buffer.concat([preBuffer, msgBuffer])
        console.log(sendBuffer)
        socket.write(sendBuffer)
    })
})

server.listen(3000)