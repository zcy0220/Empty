--[[
    @desc: 类定义
]]

local function Class(classname, super)
    local cls

    if super then
        cls = Clone(super)
        cls.super = super
    else
        cls = {}
        cls.Ctor = function(cls) end
    end

    cls.super = super
    cls.__cname = classname
    cls.__index = cls

    cls.New = function(self, ...)
        local instance = setmetatable({}, cls)
        instance.class = cls
        instance:Ctor(...)
        return instance   
    end

    return cls
end

return Class