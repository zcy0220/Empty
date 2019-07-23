--[[
    @desc:类对象池管理
]]

local ClassObjectPool = require "Base.Pool.ClassObjectPool"
local ClassObjectManager = Class("ClassObjectManager", Singleton)

-- 获得类对象池
local function GetOrCreateClassPool(self, class, maxCount)
    self.mClassPools =  self.mClassPools or {}
    self.mClassPools[class] = self.mClassPools[class] or ClassObjectPool:New(class, maxCount)
    return self.mClassPools[class]
end

ClassObjectManager.GetOrCreateClassPool = GetOrCreateClassPool

return ClassObjectManager