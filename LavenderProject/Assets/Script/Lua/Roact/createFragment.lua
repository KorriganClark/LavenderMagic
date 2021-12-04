local ElementKind = require "Roact/ElementKind"
local Type = require "Roact/Type"

local function createFragment(elements)
	return {
		[Type] = Type.Element,
		[ElementKind] = ElementKind.Fragment,
		elements = elements,
	}
end

return createFragment
