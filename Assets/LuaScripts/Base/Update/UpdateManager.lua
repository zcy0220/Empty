--[[
    @desc:管理Unity侧的Update、LateUpdate、FixedUpdate
]]

local UpdateManager = Class("UpdateManager", Singleton)

-- Update
local function Update(deltaTime)

end

-- LateUpdate
local function LateUpdate()
    -- body
end

-- FixedUpdate
local function FixedUpdate()
    -- body
end

-- 添加Update 
local function AddUpdate(self)
    self.mUpdateListenerList = self.mUpdateListenerList or {}
end

-- 添加LateUpdate
local function AddLateUpdate(self)
    self.mLateUpdateListenerList = self.mLateUpdateListenerList or {}
end

-- 添加FixedUpdate
local function AddFixedUpdate(self)
    self.mFixedUpdateListenerList = self.mFixedUpdateListenerList or {}
end

-- 删除Update
local function RemoveUpdate()
    -- body
end

-- 删除LateUpdate
local function RemoveLateUpdate()
end

-- 删除FixedUpdate
local function RemoveFixedUpdate()
end

-- 销毁
local function Dispose(self)
    self.mUpdateListenerList = nil
    self.mLateUpdateListenerList = nil
    self.mFixedUpdateListenerList = nil
end

UpdateManager.AddUpdate = AddUpdate
UpdateManager.AddLateUpdate = AddLateUpdate
UpdateManager.AddFixedUpdate = AddFixedUpdate
UpdateManager.RemoveUpdate = RemoveUpdate
UpdateManager.RemoveLateUpdate = RemoveUpdate
UpdateManager.RemoveFixedUpdate = RemoveFixedUpdate

return UpdateManager