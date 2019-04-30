--[[
    @desc: 方法句柄
]]

local function Handler(obj, method)
    return function(...)
        return method(obj, ...)
    end
end

return Handler