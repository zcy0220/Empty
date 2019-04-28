--[[
    @desc: 基础模块全局内容
    1.尽量保持和C#的目录结构一致
    2.加载顺序有一定要求
    3.模块定义时一律用local再return，模块是否是全局模块由本脚本决定，在本脚本加载的一律为全局模块
]]
----------------------------------------------------------------------
-- todo Lua层自己实现 Vector2 Vector3等常用的
Time                = CS.UnityEngine.Time

----------------------------------------------------------------------
Handler             = require "Base.Common.Handler"
Clone               = require "Base.Common.Clone"
Class               = require "Base.Common.Class"
Singleton           = require "Base.Common.Singleton"
Stack               = require "Base.Collections.Stack"
Queue               = require "Base.Collections.Queue"
ClassObjectManager  = require "Base.Pool.ClassObjectManager"

----------------------------------------------------------------------
UpdateManager       = require "Base.Update.UpdateManager"
TimerManager        = require "Base.Timer.TimerManager"