--~strict
--[[
	Packages up the internals of Roact and exposes a public API for it.
]]

--module("Roact", package.seeall)

local GlobalConfig = require "Roact/GlobalConfig"
local createReconciler = require "Roact/createReconciler"
local createReconcilerCompat = require "Roact/createReconcilerCompat"
local RobloxRenderer = require "Roact/RobloxRenderer"
local strict = require "Roact/strict"
local Binding = require "Roact/Binding"

local robloxReconciler = createReconciler(RobloxRenderer)
local reconcilerCompat = createReconcilerCompat(robloxReconciler)


local moduleName = "Roact"

local Roact = strict({
    ---组件
    Component = require "Roact/Component",
    ---创建元素
    createElement = require "Roact/createElement",
    ---创建“碎片”
    createFragment = require "Roact/createFragment",
    ---
    oneChild = require "Roact/oneChild",
    ---
    PureComponent = require "Roact/PureComponent",
    None = require "Roact/None",
    Portal = require "Roact/Portal",
    createRef = require "Roact/createRef",
    forwardRef = require "Roact/forwardRef",
    createBinding = Binding.create,
    joinBindings = Binding.join,
    createContext = require "Roact/createContext",

    Change = require "Roact/PropMarkers/Change",
    Children = require "Roact/PropMarkers/Children",
    Event = require "Roact/PropMarkers/Event",
    Ref = require "Roact/PropMarkers/Ref",

    mount = robloxReconciler.mountVirtualTree,
    unmount = robloxReconciler.unmountVirtualTree,
    update = robloxReconciler.updateVirtualTree,

    reify = reconcilerCompat.reify,
    teardown = reconcilerCompat.teardown,
    reconcile = reconcilerCompat.reconcile,

    setGlobalConfig = GlobalConfig.set,

    -- APIs that may change in the future without warning
    UNSTABLE = {},
})

_G[moduleName] = Roact

return Roact
