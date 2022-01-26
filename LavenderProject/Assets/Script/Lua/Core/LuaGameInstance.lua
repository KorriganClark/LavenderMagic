---@type Roact
Roact = require("Roact/init")
ColorUtil = require("Util/ColorUtil")

local LuaGameInstance = {}
local moduleName = "LuaGameInstance";
_G[moduleName] = LuaGameInstance;
function LuaGameInstance.Init()
    UIMgr = require("LavenderUI/UIMgr")
    UIMgr:Init()
    --CS.UnityEngine.Debug.Log("lua init")
end

function LuaGameInstance.Update()
    --CS.UnityEngine.Debug.Log("lua update")
end

return LuaGameInstance