const path = require('path')
const koa = require('koa2')
const static = require('koa-static')

// 创建koa实例
const app = new koa()

// 配置静态资源加载中间件
app.use(static(path.join(__dirname, './public')))

app.listen(8000)