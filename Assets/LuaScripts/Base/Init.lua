--[[
    @desc: 基础模块全局内容
    1.尽量保持和C#的目录结构一致
    2.模块定义时一律用local再return，模块是否是全局模块由本脚本决定，在本脚本加载的一律为全局模块
]]

Handler = require "Base.Common.Handler"
Clone = require "Base.Common.Clone"
Class = require "Base.Common.Class"
Singleton = require "Base.Common.Singleton"

UpdateManager = require "Base.Update.UpdateManager"