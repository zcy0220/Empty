--[[
    @desc:定时器管理
]]

local TimerManager = Class("TimerManager", Singleton)

-- 构造
local function Ctor(self)
    self.mTimerSet = {}
    self.mTimerPool = ClassObjectManager:Instance():GetOrCreateClassPool("Base.Timer.Timer")
end

-- 创建定时器
local function CreateTimer(self, duration, finishAction, loop, unScale)
    local timer = self.mTimerPool:Spawn()
    timer:Init(duration, finishAction, loop, unScale)
    timer:Start()
    self.mTimerSet[timer] = true
end

-- 定时更新
local function Update(self, deltaTime)
    for timer, v in pairs(self.mTimerSet) do
        timer:Update(deltaTime)
        if timer:IsOver() then
            self.mRemoveTimerList = self.mRemoveTimerList or {}
            table.insert(self.mRemoveTimerList, timer)
        end
    end
    if self.mRemoveTimerList then
        for i, timer in ipairs(self.mRemoveTimerList) do
            self.mTimerSet[timer] = nil
            self.mTimerPool:Recycle(timer)
        end
        self.mRemoveTimerList = nil
    end
end

TimerManager.Ctor = Ctor
TimerManager.CreateTimer = CreateTimer
TimerManager.Update = Update

return TimerManager