local Type = require "Roact/Type"
local Symbol = require "Roact/Symbol"

local function noop()
	return nil
end

local ElementUtils = {}

--[[
	A signal value indicating that a child should use its parent's key, because
	it has no key of its own.

	This occurs when you return only one element from a function component or
	stateful render function.
]]
ElementUtils.UseParentKey = Symbol.named("UseParentKey")

--[[
	Returns an iterator over the children of an element.
	`elementOrElements` may be one of:
	* a boolean
	* nil
	* a single element
	* a fragment
	* a table of elements

	If `elementOrElements` is a boolean or nil, this will return an iterator with
	zero elements.

	If `elementOrElements` is a single element, this will return an iterator with
	one element: a tuple where the first value is ElementUtils.UseParentKey, and
	the second is the value of `elementOrElements`.

	If `elementOrElements` is a fragment or a table, this will return an iterator
	over all the elements of the array.

	If `elementOrElements` is none of the above, this function will throw.
]]
function ElementUtils.iterateElements(elementOrElements)
	local richType = Type.of(elementOrElements)

	-- Single child
	if richType == Type.Element then
		local called = false

		return function()
			if called then
				return nil
			else
				called = true
				return ElementUtils.UseParentKey, elementOrElements
			end
		end
	end

	local regularType = type(elementOrElements)

	if elementOrElements == nil or regularType == "boolean" then
		return noop
	end

	if regularType == "table" then
		return pairs(elementOrElements)
	end

	error("Invalid elements")
end

--[[
	Gets the child corresponding to a given key, respecting Roact's rules for
	children. Specifically:
	* If `elements` is nil or a boolean, this will return `nil`, regardless of
		the key given.
	* If `elements` is a single element, this will return `nil`, unless the key
		is ElementUtils.UseParentKey.
	* If `elements` is a table of elements, this will return `elements[key]`.
]]
function ElementUtils.getElementByKey(elements, hostKey)
	if elements == nil or type(elements) == "boolean" then
		return nil
	end

	if Type.of(elements) == Type.Element then
		if hostKey == ElementUtils.UseParentKey then
			return elements
		end

		return nil
	end

	if type(elements) == "table" then
		return elements[hostKey]
	end

	error("Invalid elements")
end


---用于对应C#中的Element类型
local ElementType = {
	Null = 0,
	Text = 1,
	Image = 2,
	Button = 3
}

---@param elementType string
---@return
function ElementUtils.newElementInstance(elementType)
	return CS.Lavender.UI.LUIElement.NewElement(ElementType[elementType])
end

return ElementUtils
