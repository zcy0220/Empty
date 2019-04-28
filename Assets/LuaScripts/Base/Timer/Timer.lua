--[[
    @desc:定时器
]]

local Timer = Class("Timer")

-- 初始化定时器参数
local function Init(self, duration, finishAction, loop, unScale)
    self.mDuration = duration
    self.mLoop = loop or 1
    self.mUnScale = unScale or false
    self.mFinishAction = finishAction
    self.mRunning = false
    self.mIsOver = true
end

-- 启动
local function Start(self)
    self.mLeftTime = self.mDuration
    self.mStartRealTime = Time.realtimeSinceStartup
    self.mRunning = true
    self.mIsOver = false
end

-- 暂停
local function Pause(self)
    self.mRunning = false
end

-- 恢复
local function Resume(self)
    self.mRunning = true
end

-- 停止
local function Stop(self)
    self.mLeftTime = 0
    self.mRunning = false
    self.mIsOver = true
end

-- 更新
local function Update(self, deltaTime)
    if not self.mRunning then return end

    -- 要用真实时间时：unscaledDeltaTime 和 通过Time.realtimeSinceStartup - mStartRealTime
    -- 测试结果，二者几乎就是一样的 todo 真机测试
    if self.mUnScale then deltaTime = Time.realtimeSinceStartup - self.mStartRealTime end

    self.mLeftTime = self.mLeftTime - deltaTime
    if self.mLeftTime <= 0 then
        if self.mFinishAction then self.mFinishAction() end

        if self.mLoop > 0 then
            self.mLoop = self.mLoop - 1
            self.mLeftTime = self.mLeftTime + self.mDuration
        end

        if self.mLoop == 0 then
            self:Stop()
            return
        end

        if self.mLoop < 0 then
            self.mLeftTime = self.mLeftTime + self.mDuration
        end
    end
end

-- 是否停止
local function IsOver(self)
    return self.mIsOver
end

Timer.Init = Init
Timer.Start = Start
Timer.Pause = Pause
Timer.Resume = Resume
Timer.Stop = Stop
Timer.Update = Update
Timer.IsOver = IsOver

return Timer