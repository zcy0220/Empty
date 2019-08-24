--[[
    @desc:单例类
]]

local Singleton = Class("Singleton")

-- 原始访问自身的mInstance
local function Instance(self)
    if not rawget(self, "mInstance") then
        rawset(self, "mInstance", self:New())
    end
    return self.mInstance
end

Singleton.Instance = Instance

return Singleton

