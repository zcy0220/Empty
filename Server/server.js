const net = require('net')
var protobuf = require("protobufjs")
var root = protobuf.loadSync("./proto/Example.proto")
var message = root.lookupType("Proto.Example")

const server = net.createServer((socket) => {
    socket.on('data', (buffer) => {
        console.log(message.decode(buffer))
        // socket.emit('', buffer)  
    })
})

server.listen(3000)