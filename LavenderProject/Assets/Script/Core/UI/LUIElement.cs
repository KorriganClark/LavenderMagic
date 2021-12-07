using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

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
                    var localPositionin = new Vector2(((RectTransform)go.transform).localPosition.x, ((RectTransform)go.transform).localPosition.y);
                    var tableAnchorMin = (LuaTable)prop;
                    var vectvalAnchorMin = new Vector2(tableAnchorMin.Get<float>("x"), tableAnchorMin.Get<float>("y"));
                    ((RectTransform)go.transform).anchorMin = vectvalAnchorMin;
                    ((RectTransform)go.transform).localPosition = localPositionin;
                    return;
                case "anchorMax":
                    var localPositionax = new Vector2(((RectTransform)go.transform).localPosition.x, ((RectTransform)go.transform).localPosition.y);
                    var tableAnchorMax = (LuaTable)prop;
                    var vectvalAnchorMax = new Vector2(tableAnchorMax.Get<float>("x"), tableAnchorMax.Get<float>("y"));
                    ((RectTransform)go.transform).anchorMax = vectvalAnchorMax;
                    ((RectTransform)go.transform).localPosition = localPositionax;
                    return;
                case "position":
                    var tablePos = (LuaTable)prop;
                    var vectPos = new Vector2(tablePos.Get<float>("x"), tablePos.Get<float>("y"));
                    ((RectTransform)go.transform).localPosition = vectPos;
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

        
    }
}

