--[[
    @desc: Lua日志输出
]]

local Debugger = Class("Debugger")

local function Log(message)
    if Config.Debug then
        CS.UnityEngine.Debug.Log(message)
    end
end

local function LogError(message)
    if Config.Debug then
        CS.UnityEngine.Debug.LogError(message)
    end
end

Debugger.Log = Log
Debugger.LogError = LogError

return Debugger