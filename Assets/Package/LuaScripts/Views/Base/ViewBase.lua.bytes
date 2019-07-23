--[[
    @desc:ViewBase
]]

local ViewBase = Class("ViewBase")
local ID = 0

-- 唯一Id
local function Id(self)
    return self.mId
end

-- 构造 数据初始化
local function Ctor(self, gameObject)
    ID = ID + 1
    self.mId = ID
    self.mGameObject = gameObject
    self.mRectTransform = gameObject:GetComponent("RectTransform")
    self:BindComponents(self.mGameObject.transform)
end

-- 创建 UI控件初始化
local function Create(self, data)
end

-- 设置UI名字
local function SetName(self, name)
    self.mName = name
end

-- 获得界面名字
local function GetName(self, name)
    return self.mName
end

-- 关闭
local function Close(self)
    if self.mChilds then
        for i,v in ipairs(self.mChilds) do
            v:Close()
        end
    end
    self.mChilds = nil
    self:ClearAllEventListener()
    ViewManager:Instance():RemoveById(self.mId)
    GameObject.DestroyImmediate(self.mGameObject)
end

-- 添加子节点
local function AddChild(self, name, data)
    local child = ViewManager:Instance():Load(name)
    child:Create(data)
    child:SetParent(self)
    self.mChilds = self.mChilds or {}
    table.insert(self.mChilds, child)
end

-- 设置父节点
local function SetParent(self, parent)
    local parentGo = parent:GetGameObject()
    self.mGameObject.transform:SetParent(parentGo.transform)
end

-- 返回GameObject节点
local function GetGameObject(self)
    return self.mGameObject
end

-- 绑定按钮控件
local function BindComponents(self, transform)
    local stack = Stack:New()
    stack:Push({node = transform, name = ""})
    while not stack:Empty() do
        local data = stack:Pop()
        local node = data.node
        local preName = data.name
        for i=0, node.childCount - 1 do
            local child = node:GetChild(i)
            local name = child.gameObject.name
            local component
            if string.startsWith(name, "Btn") then
                component = child:GetComponent("Button")
                local cb = self["On" .. name]
                if cb then
                    component.onClick:AddListener(Handler(self, cb))
                end
            elseif string.startsWith(name, "Img") then
                component = child:GetComponent("Image")
            elseif string.startsWith(name, "Text") then
                component = child:GetComponent("Text")
            elseif string.startsWith(name, "Input") then
                component = child:GetComponent("InputField")
            elseif string.startsWith(name, "Scroll") then
                component = child:GetComponent("ScrollRect")
            elseif string.startsWith(name, "Slider") then
                component = child:GetComponent("Slider")
            else
                component = child
            end
            if component then
                self[preName .. name] = component
            end
            stack:Push({node = child, name = name .. "/"})
        end
    end
end

-- 设置层级
local function SetSiblingIndex(self, siblingIndex)
    self.mGameObject.transform:SetSiblingIndex(siblingIndex)
end

-- 添加监听事件
local function AddEventListener(self, eventName, listener)
    self.mListeners = self.mListeners or {}
    EventManager:Instance():AddEventListener(eventName, listener)
    table.insert(self.mListeners, {n = eventName, l = listener})
end

-- 清空自身所有监听事件
local function ClearAllEventListener(self)
    if self.mListeners == nil then return end
    for k,v in pairs(self.mListeners) do
        EventManager:Instance():RemoveEventListener(v.n, v.l)
    end
    self.mListeners = nil
end

ViewBase.Id = Id
ViewBase.Ctor = Ctor
ViewBase.Create = Create
ViewBase.SetName = SetName
ViewBase.GetName = GetName
ViewBase.Close = Close
ViewBase.AddChild = AddChild
ViewBase.SetParent = SetParent
ViewBase.GetGameObject = GetGameObject
ViewBase.BindComponents = BindComponents
ViewBase.SetSiblingIndex = SetSiblingIndex
ViewBase.AddEventListener = AddEventListener
ViewBase.ClearAllEventListener = ClearAllEventListener

return ViewBase