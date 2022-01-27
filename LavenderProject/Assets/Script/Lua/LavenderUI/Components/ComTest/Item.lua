--this file is gen by script
--you can edit this file in custom part


--lua fields
local Item = Roact.Component:extend("Item")
Item.state = {
	test1 = {},
	test2 = {},
}
Item.slot = {
	test1 = {},
	test2 = {},
}
Item.props = {
	test1 = {},
	test2 = {},
}
--lua fields end

--lua functions
function Item:dataBind()
	
	self.slot = self.state
	
end --func end
--next--
function Item:buildTree()
	
	return Roact.createElement("Image",{
		color = CS.UnityEngine.Color.black,
	},{
		Text = Roact.createElement("Text",{
			text = "New Text",
			color = CS.UnityEngine.Color.black,
		},{
			
		}),
		Image = Roact.createElement("Image",{
			color = CS.UnityEngine.Color.black,
		},{
			
		}),
		
	})
	
end --func end
--next--
function Item:init()
	
	self:setState(Item.state)
	
end --func end
--next--
function Item:render()
	
	self.props = Item.props
	Item.dataBind(self)
	return Item.buildTree(self)
	
end --func end
--next--
--lua functions end

--lua custom scripts

--lua custom scripts end
return Item