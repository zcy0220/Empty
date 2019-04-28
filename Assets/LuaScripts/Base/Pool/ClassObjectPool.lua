--[[
    @desc:类对象池
]]

local ClassObjectPool = Class("ClassObjectPool")

-- maxCount -1表示不限个数
local function Ctor(self, class, maxCount)
    self.mPool = Stack:New()
    self.mClass = require(class)
    self.mMaxCount = maxCount or -1
end

-- 分配对象
local function Spawn(self)
    if self.mPool:Count() > 0 then
        print("现有")
        return self.mPool:Pop()
    else
        print("创新")
        return self.mClass:New()
    end
end

-- 回收对象
local function Recycle(self, obj)
    if obj == nil then return end
    if self.mMaxCount > 0 and self.mPool:Count() >= self.mMaxCount then
        obj = nil
        return
    end
    self.mPool:Push(obj)
end

-- 清空对象池
local function Clear(self)
    self.mPool:Clear()
end

ClassObjectPool.Ctor = Ctor
ClassObjectPool.Spawn = Spawn
ClassObjectPool.Recycle = Recycle
ClassObjectPool.Clear = Clear

return ClassObjectPool