--[[
    @desc:ViewManager
]]

local ViewConfig = require "Views.ViewConfig"
local ViewManager = Class("ViewManager", Singleton)

-- 初始化
local function Ctor(self)
    self.mStack = Stack:New()
    self.mCurSiblingIndex = 0
end

-- 加载界面
local function Load(self, name)
    local prefab = ResourceManager:SyncLoadGameObject(ViewConfig[name])
    local gameObject = GameObject.Instantiate(prefab)
    self:AddChild(gameObject.transform)
    local class = require(name)
    local view = class:New(gameObject)
    view:SetName(name)
    self.mViews = self.mViews or {}
    self.mViews[view:Id()] = view
    return view
end

-- 公告，网络异常转圈等置顶的UI  
local function Add(self, name, data, siblingIndex)
    siblingIndex = siblingIndex or 100
    local view = self:Load(name)
    view:Create(data)
    view:SetSiblingIndex(siblingIndex)
    return view
end

-- 根据名字删除界面 同一名字的所有界面都删除
local function RemoveByName(self, name)
    local removeList
    for k,v in pairs(self.mViews) do
        if v:GetName() == name then
            removeList = removeList or {}
            table.insert(removeList, v)
        end
    end
    if removeList then
        for i,v in ipairs(removeList) do
            v:Close()
        end
    end
end

-- 根据Id删除界面
local function RemoveById(self, id)
    if self.mViews and self.mViews[id] then
        self.mViews[id] = nil
    end
end

-- 入栈
local function Push(self, name, data)
    local view = self:Load(name)
    view:Create(data)
    view:SetSiblingIndex(self.mCurSiblingIndex)
    self.mCurSiblingIndex = self.mCurSiblingIndex + 1
    self.mStack:Push(view)
end

-- 出栈
local function Pop(self)
    if self.mStack:Empty() then return end
    local view = self.mStack:Pop()
    self.mCurSiblingIndex = self.mCurSiblingIndex - 1
    view:Close()
end

-- 替换
local function Replace(self, name, data)
    while not self.mStack:Empty() do
        self:Pop()
    end
    self:Push(name, data)
end

-- 添加界面
local function AddChild(self, child)
    if not child then return end
    child:SetParent(self:GetViewRoot(), false)
end

-- 返回主Canvas
local function GetViewRoot(self)
    self.ViewRoot = self.ViewRoot or GameObject.Find("Canvas").transform
    return self.ViewRoot
end

-- 施放清理界面
local function Clear(self)
    for k,v in pairs(self.mViews) do
        v:Close()
    end
    self.mStack:Clear()
    self.mCurSiblingIndex = 0
    self.mViews = nil
end

ViewManager.Ctor = Ctor
ViewManager.Load = Load
ViewManager.Add = Add
ViewManager.RemoveByName = RemoveByName
ViewManager.RemoveById = RemoveById
ViewManager.Push = Push
ViewManager.Pop = Pop
ViewManager.Replace = Replace
ViewManager.AddChild = AddChild
ViewManager.GetViewRoot = GetViewRoot
ViewManager.Clear = Clear

return ViewManager