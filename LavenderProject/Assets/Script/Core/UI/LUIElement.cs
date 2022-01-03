using Lavender.Lua;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;
using static Lavender.UI.LUITree;

namespace Lavender.UI
{
    public enum ElementType
    {
        Null,
        Text,
        Image,
        Button
    }

    

    public static class LUIElement
    {
        public static string[] elementTypeString = { "Null", "Text" ,"Image", "Button"};
        //public ElementType elementType;
        [LuaCallCSharp]
        public static GameObject NewElement(ElementType type)
        {
            switch (type)
            {
                case ElementType.Text:
                    return LTextElement.NewElement();
                case ElementType.Image:
                    return LImageElement.NewElement();
                case ElementType.Button:
                    return LButtonElement.NewElement();
                default: break;
            }
            return null;
        }

        public interface IBaseProps
        {
            string name { get; set; }
            Vector3 localPosition { get; set; }
        }

        public static void SetProperty(GameObject go, string key, object prop)
        {
            if(prop == null)
            {
                return;
            }
            switch (key)
            {
                case "size":
                    var table = (LuaTable)prop;
                    var vectval = new Vector2(table.Get<float>("x"), table.Get<float>("y"));
                    ((RectTransform)go.transform).sizeDelta = vectval;
                    return;
                case "anchorMin":
                    var localPositionin = new Vector2(((RectTransform)go.transform).anchoredPosition.x, ((RectTransform)go.transform).anchoredPosition.y);
                    var tableAnchorMin = (LuaTable)prop;
                    var vectvalAnchorMin = new Vector2(tableAnchorMin.Get<float>("x"), tableAnchorMin.Get<float>("y"));
                    ((RectTransform)go.transform).anchorMin = vectvalAnchorMin;
                    return;
                case "anchorMax":
                    var localPositionax = new Vector2(((RectTransform)go.transform).anchoredPosition.x, ((RectTransform)go.transform).anchoredPosition.y);
                    var tableAnchorMax = (LuaTable)prop;
                    var vectvalAnchorMax = new Vector2(tableAnchorMax.Get<float>("x"), tableAnchorMax.Get<float>("y"));
                    ((RectTransform)go.transform).anchorMax = vectvalAnchorMax;
                    return;
                case "position":
                    var tablePos = (LuaTable)prop;
                    var vectPos = new Vector3(tablePos.Get<float>("x"), tablePos.Get<float>("y"), 0);
                    ((RectTransform)go.transform).anchoredPosition3D = vectPos;
                    Debug.Log(((RectTransform)go.transform).anchoredPosition);
                    Debug.Log(((RectTransform)go.transform).position);//bug 原因，在设置位置时，还没挂载到父节点上，导致以世界为坐标进行设置，在设置属性前需要先挂载父节点。

                    return;
            }

            var type = LUIMgr.GetElementType(go);
            switch (type)
            {
                case ElementType.Text:
                    LTextElement.SetProperty(go, key, prop);break;
                case ElementType.Image:
                    LImageElement.SetProperty(go, key, prop);break;
                case ElementType.Button:
                    LButtonElement.SetProperty(go, key, prop);break;
                default: break;
            }
        }

        public static void GenLuaProperty(UINode node, StringBuilder builder, string nextLine)
        {
            switch (node.NodeType)
            {
                case ElementType.Text:
                    LTextElement.GenLuaProperty(node, builder, nextLine); break;
                case ElementType.Image:
                    LImageElement.GenLuaProperty(node, builder, nextLine); break;
                case ElementType.Button:
                    //LButtonElement.GenLuaProperty(node, builder, nextLine); break;
                default: break;
            }
        }

    }
}

