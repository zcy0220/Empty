--[[
    @desc:管理Unity侧的Update、LateUpdate、FixedUpdate
]]

local UpdateManager = Class("UpdateManager", Singleton)

-- Update
local function Update(self, deltaTime, unscaledDeltaTime)
    if self.mUpdateListenerList then
        for i, update in ipairs(self.mUpdateListenerList) do
            update(deltaTime, unscaledDeltaTime)
        end
    end
end

-- LateUpdate
local function LateUpdate(self)
    if self.mLateUpdateListenerList then
        for i, lateUpdate in ipairs(self.mLateUpdateListenerList) do
            lateUpdate()
        end
    end
end

-- FixedUpdate
local function FixedUpdate(self, fixedDeltaTime)
    if self.mFixedUpdateListenerList then
        for i, fixedUpdate in ipairs(self.mFixedUpdateListenerList) do
            fixedUpdate(fixedDeltaTime)
        end
    end
end

-- 添加Update 
local function AddUpdate(self, listener)
    self.mUpdateListenerList = self.mUpdateListenerList or {}
    table.insert(self.mUpdateListenerList, listener)
end

-- 添加LateUpdate
local function AddLateUpdate(self, listener)
    self.mLateUpdateListenerList = self.mLateUpdateListenerList or {}
    table.insert(self.mLateUpdateListenerList, listener)
end

-- 添加FixedUpdate
local function AddFixedUpdate(self, listener)
    self.mFixedUpdateListenerList = self.mFixedUpdateListenerList or {}
    table.insert(self.mFixedUpdateListenerList, listener)
end

-- 删除Update
local function RemoveUpdate(self, listener)
    self.mRemoveUpdateList = self.mRemoveUpdateList or {}
    table.insert(self.mRemoveUpdateList, listener)
end

-- 删除LateUpdate
local function RemoveLateUpdate(self, listener)
    self.mRemoveLateUpdateList = self.mRemoveLateUpdateList or {}
    table.insert(self.mRemoveLateUpdateList, listener)
end

-- 删除FixedUpdate
local function RemoveFixedUpdate(self, listener)
    self.mRemoveFixedUpdateList = self.mRemoveFixedUpdateList or {}
    table.insert(self.mRemoveFixedUpdateList, listener)
end

-- 销毁
local function Dispose(self)
    self.mUpdateListenerList = nil
    self.mLateUpdateListenerList = nil
    self.mFixedUpdateListenerList = nil
    self.mRemoveUpdateList = nil
    self.mRemoveLateUpdateList = nil
    self.mRemoveFixedUpdateList = nil
end

UpdateManager.Update = Update
UpdateManager.LateUpdate = LateUpdate
UpdateManager.FixedUpdate = FixedUpdate
UpdateManager.AddUpdate = AddUpdate
UpdateManager.AddLateUpdate = AddLateUpdate
UpdateManager.AddFixedUpdate = AddFixedUpdate
UpdateManager.RemoveUpdate = RemoveUpdate
UpdateManager.RemoveLateUpdate = RemoveUpdate
UpdateManager.RemoveFixedUpdate = RemoveFixedUpdate
UpdateManager.Dispose = Dispose

return UpdateManager