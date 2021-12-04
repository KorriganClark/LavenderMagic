require("Roact/init")

local LuaGameInstance = {}
local moduleName = "LuaGameInstance";
_G[moduleName] = LuaGameInstance;
function LuaGameInstance.Init()
    CS.UnityEngine.Debug.Log("lua init")
end

function LuaGameInstance.Update()
    CS.UnityEngine.Debug.Log("lua update")
end

return LuaGameInstance