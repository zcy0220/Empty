--[[
    @desc:堆栈
]]

local Stack = Class("Stack")

-- 构造
local function Ctor(self)
    self.mTop = 0
    self.mList = {}
end

-- 入栈
local function Push(self, element)
    self.mTop = self.mTop + 1
    self.mList[self.mTop] = element
end

-- 出栈
local function Pop(self)
    if self.mTop > 0 then
        local element = self.mList[self.mTop]
        self.mList[self.mTop] = nil
        self.mTop = self.mTop - 1
        return element
    end
end

-- 获取栈顶元素
local function Peek(self)
    return self.mList[self.mTop]
end

-- 判空
local function Empty(self)
    return self.mTop == 0
end

-- 数量
local function Count(self)
    return self.mTop
end

-- 清空
local function Clear(self)
    self.mTop = 0
    self.mList = {}
end

Stack.Ctor = Ctor
Stack.Push = Push
Stack.Pop = Pop
Stack.Peek = Peek
Stack.Empty = Empty
Stack.Count = Count
Stack.Clear = Clear

return Stack