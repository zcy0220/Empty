--[[
    @desc:字符串拓展
]]

-- 头尾去空
local function trim(target)
	return string.gsub(target, "^%s*(.-)%s*$", "%1")
end

-- 以某个字符串开始
local function startsWith(target, pattern, plain)
	local findBegin = string.find(target, pattern, 1, plain)
	return findBegin == 1
end

-- 以某个字符串结尾
local function endsWith(target, pattern, plain)
	local findEnd = string.find(target, pattern, 1, plain)
	return findEnd == #target - #pattern + 1
end

-- 计算UTF8字符串的长度，每一个中文算一个字符
local function utf8len(target)
    local len  = string.len(target)
    local left = len
    local cnt  = 0
    local arr  = {0, 0xc0, 0xe0, 0xf0, 0xf8, 0xfc}
    while left ~= 0 do
        local tmp = string.byte(target, -left)
        local i   = #arr
        while arr[i] do
            if tmp >= arr[i] then
                left = left - i
                break
            end
            i = i - 1
        end
        cnt = cnt + 1
    end
    return cnt
end

string.trim = trim
string.startsWith = startsWith
string.endsWith = endsWith
string.utf8len = utf8len