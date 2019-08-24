--[[
    @desc:队列
]]

local Queue = Class("Queue")

-- 构造
local function Ctor(self)
    self.mHead = 1
    self.mTail = 0
    self.mList = {}
end

-- 入队列
local function Enqueue(self, element)
    self.mTail = self.mTail + 1
    self.mList[self.mTail] = element
end

-- 出队列
local function Dequeue(self)
    if not self:Empty() then
        local element = self.mList[self.mHead]
        self.mList[self.mHead] = nil
        self.mHead = self.mHead + 1
        return element
    end
end

-- 判空
local function Empty(self)
    return self.mHead > self.mTail
end

-- 总数
local function Count(self)
    return self.mTail - self.mHead + 1
end

-- 清空
local function Clear(self)
    self.mHead = 1
    self.mTail = 0
    self.mList = {}
end

Queue.Ctor = Ctor
Queue.Enqueue = Enqueue
Queue.Dequeue = Dequeue
Queue.Empty = Empty
Queue.Count = Count
Queue.Clear = Clear

return Queue