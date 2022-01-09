
---@class BagItem
local BagItem = Roact.Component:extend("BagItem")

--初始state
BagItem.state = {
    ItemId = 0
}
BagItem.slot = {
    ItemId = 0
}
--初始props
BagItem.props = {
    position = { x = 100, y = 100}
}


--custom dataBind
function BagItem:dataBind()
    self.slot = self.state
end

--构建 UI 树
function BagItem:buildTree()
    return Roact.createElement("Image",{
        anchorMin = {x = 0, y = 1},
        anchorMax = {x = 0, y = 1},
        position = self.props.position
    },{
        Text = Roact.createElement("Text",{
            size = {x = 100, y = 100},
            text = "Item!!!" .. self.slot.ItemId,
            color = CS.UnityEngine.Color.black,
        })
    })
end

--初始化函数
function BagItem:init()
    self:setState(BagItem.state)
end

--render 在这里做 props 和 state 的解析与绑定
function BagItem:render()
    self.props = BagItem.props --TODO，设置默认的props，当没有传入的属性时，按照该属性设置
    BagItem.dataBind(self)
    return BagItem.buildTree(self)
end

return BagItem