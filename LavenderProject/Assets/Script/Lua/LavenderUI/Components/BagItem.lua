---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by gancheng.
--- DateTime: 2021/12/6 15:36
---

local BagItem = Roact.Component:extend("BagItem")

function BagItem:init()
    self:setState({
        ItemId = 0
    })
end

function BagItem:render()
    local ItemId = self.state.ItemId

    return Roact.createElement("Image",{

    },{
        Text = Roact.createElement("Text",{
            size = {x = 100, y = 100},
            text = "Item!!!",
            color = CS.UnityEngine.Color.black
        })
    })
end

return BagItem