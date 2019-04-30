--[[
    @desc: 基础模块全局内容
    1.尽量保持和C#的目录结构一致
    2.加载顺序有一定要求
        - C#侧和Unity侧常用类
        - 基础全局方法和数据结构
        - 基础模块（会调用以上两点）
    3.模块定义时一律用local再return，模块是否是全局模块由本脚本决定，在本脚本加载的一律为全局模块
]]
----------------------------------------------------------------------
-- todo Lua层自己实现 Vector2 Vector3等常用的
Time                = CS.UnityEngine.Time

----------------------------------------------------------------------
Handler             = require "Base.Common.Handler"
Clone               = require "Base.Common.Clone"
Class               = require "Base.Common.Class"
Stack               = require "Base.Collections.Stack"
Queue               = require "Base.Collections.Queue"
Debugger            = require "Base.Debug.Debugger"
Singleton           = require "Base.Common.Singleton"
ClassObjectManager  = require "Base.Pool.ClassObjectManager"

----------------------------------------------------------------------
EventManager        = require "Base.Event.EventManager"
UpdateManager       = require "Base.Update.UpdateManager"
TimerManager        = require "Base.Timer.TimerManager"