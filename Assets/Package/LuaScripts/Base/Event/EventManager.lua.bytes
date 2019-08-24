--[[
    @desc:事件管理类
]]

local EventManager = Class("EventManager", Singleton)

-- 添加监听事件
local function AddEventListener(self, eventName, listener)
    self.mListeners = self.mListeners or {}
    self.mListeners[eventName] = self.mListeners[eventName] or {}
    table.insert(self.mListeners[eventName], listener)
end

-- 删除监听事件
local function RemoveEventListener(self, eventName, listener)
    self.mListeners = self.mListeners or {}
    if not self.mListeners[eventName] then return end
    for k,v in pairs(self.mListeners[eventName]) do
        if listener == v then
            self.mListeners[eventName][k] = nil
            break
        end
    end
end

-- 触发监听事件
local function PostEvent(self, eventName, params)
    self.mListeners = self.mListeners or {}
    if not self.mListeners[eventName] then return end
    for k, listener in pairs(self.mListeners[eventName]) do
        listener(params)
    end
end

EventManager.AddEventListener = AddEventListener
EventManager.RemoveEventListener = RemoveEventListener
EventManager.PostEvent = PostEvent

return EventManager