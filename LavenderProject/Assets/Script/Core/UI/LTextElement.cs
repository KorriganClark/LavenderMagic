using Lavender.Lua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Lavender.UI
{
    
    public class LTextElement 
    {
        public static ElementType elementType = ElementType.Text;

        public class TextProps : LUIElement.BaseProps
        {

            string text { get; set; }
            Font font { get; set; }

        }
        public TextProps props;
        public static GameObject NewElement()
        {
            GameObject go = new GameObject("new_Text", typeof(Text));
            var text = go.GetComponent<Text>();
            text.raycastTarget = false;
            text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            text.text = "New Text";
            go.transform.SetParent(LUIMgr.SecurityCheck().transform);
            go.transform.localPosition = new Vector3(0, 0, 0);
            LUIMgr.SetElementType(go, ElementType.Text);
            return go;
        }

        

        public static void SetProperty(GameObject go, string key, object prop)
        {
            var textComp = go.GetComponent<Text>();
            switch(key)
            {
                case "text":
                    textComp.text = (string)prop;
                    break;
                case "color":
                    textComp.color = (Color)prop;
                    break;
            }
        }
        
        public static void GenLuaProperty(GameObject node, StringBuilder builder, string nextLine)
        {
            var textComp = node.GetComponent<Text>();
            builder.Append(nextLine).Append("text = ").Append($"\"{textComp.text}\",");
            builder.Append(nextLine).Append("color = ").Append($"{textComp.color.ToString()},");
        }
    }
}
