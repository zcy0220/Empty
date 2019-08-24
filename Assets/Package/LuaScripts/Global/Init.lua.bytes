--[[
    @desc: 全局的初始化配置
    1.所有全局模块必须要在这里加载，好集中管理
    2.这里的全局模块是和逻辑相关的，逻辑无关的全局内容放在Base/Init.lua
    3.模块定义时一律用local再return，模块是否是全局模块由本脚本决定，在本脚本加载的一律为全局模块
]]

-- 基础模块全局初始化
require "Base.Init"

----------------------------------------------------------------------
--[[
    全局常量配置
]]
GlobalWords          = require "Global.GlobalWords"
GlobalEvent          = require "Global.GlobalEvent"

----------------------------------------------------------------------
--[[
    C#侧、Unity侧相关
]]
ResourceManager     = CS.ResourceManager.Instance

----------------------------------------------------------------------
--[[
    UI视图相关
]]
ViewBase            = require "Views.Base.ViewBase"
ViewManager         = require "Views.ViewManager"