using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static Lavender.UI.LUITree;

namespace Lavender.UI
{
    public static class LImageElement
    {
        public static ElementType elementType = ElementType.Image;

        public static GameObject NewElement()
        {
            GameObject go = new GameObject("new_Image", typeof(Image));
            var image = go.GetComponent<Image>();
            image.raycastTarget = false;
            go.transform.SetParent(LUIMgr.SecurityCheck().transform);
            go.transform.localPosition = new Vector3(0, 0, 0);
            LUIMgr.SetElementType(go, ElementType.Image);
            return go;
        }

        public static void SetProperty(GameObject go,string key,object prop)
        {
            var imageComp = go.GetComponent<Image>();
            switch (key)
            {
                case "sprite":break;
                case "color":
                    imageComp.color = (Color)prop;
                    break;

            }
        }
        /*
        public static void GenLuaProperty(UINode node, StringBuilder builder, string nextLine)
        {
            var image = (UINodeImagePart)node.ScriptInstance;
            //builder.Append("text = ").Append($"{text.TextString}");
            builder.Append(nextLine).Append("color = ").Append($"{image.ImageColor},");
        }*/
    }
}
