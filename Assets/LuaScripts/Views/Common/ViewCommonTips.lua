--[[
	@desc:ViewCommonTips
]]

local ViewCommonTips = Class("ViewCommonTips", ViewBase)

-- 构造 数据初始化
local function Ctor(self, gameObject)
	ViewCommonTips.super.Ctor(self, gameObject)
end

-- 创建 UI控件初始化
local function Create(self, data)
	ViewCommonTips.super.Create(self)
	-- 传入Id，显示GlobalWords，否则显示该字符串
	local word = GlobalWords[data] or data
	self["ImgContentBg/TextContent"].text = word
	TimerManager:Instance():CreateTimer(2, Handler(self, self.Close))
end

-- 关闭
local function Close(self)
	ViewCommonTips.super.Close(self)
end

ViewCommonTips.Ctor = Ctor
ViewCommonTips.Create = Create
ViewCommonTips.Close = Close

return ViewCommonTips
