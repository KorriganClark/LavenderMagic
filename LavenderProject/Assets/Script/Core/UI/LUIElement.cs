using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

namespace Lavender.UI
{
    public enum ElementType
    {
        Text,
        Image
    }

    public static class LUIElement
    {
        //public ElementType elementType;
        [LuaCallCSharp]
        public static GameObject NewElement(ElementType type)
        {
            //Debug.Log("new Element!!");
            switch (type)
            {
                case ElementType.Text:
                    return LTextElement.NewElement();
                case ElementType.Image:break;
                default: break;
            }
            return null;
        }

        
    }
}

