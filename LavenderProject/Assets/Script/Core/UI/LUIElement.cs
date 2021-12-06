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
            switch (key)
            {
                case "size":
                    var table = (LuaTable)prop;
                    var vectval = new Vector2(table.Get<float>("x"), table.Get<float>("y"));
                    ((RectTransform)go.transform).sizeDelta = vectval;
                    return;
            }

            var type = LUIMgr.GetElementType(go);
            switch (type)
            {
                case ElementType.Text:
                    LTextElement.SetProperty(go, key, prop);break;
                case ElementType.Image:
                    LImageElement.SetProperty(go);break;
                case ElementType.Button:
                    LButtonElement.SetProperty(go);break;
                default: break;
            }
        }

        
    }
}

