--[[
    @desc: Lua程序主入口
]]

require "Config"
require "Global.Init"

GameMain = {}

-- lua开始执行入口
local function Start()
    print("Lua GameMain Start ...")
end

-- 销毁
local function Dispose()
    UpdateManager:Instance():Dispose()
end

GameMain.Start = Start
GameMain.Dispose = Dispose

--------------------------------------------------------------------------------------------------------
-- C# LuaUpdater调用(这里混了一个全局方法不是很好，但主要是放在其他地方不直观)
function Update(deltaTime, unscaledDeltaTime)
    UpdateManager:Instance():Update(deltaTime, unscaledDeltaTime)
end