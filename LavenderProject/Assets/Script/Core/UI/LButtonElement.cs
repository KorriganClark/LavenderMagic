using UnityEngine;
using UnityEngine.UI;

namespace Lavender.UI
{
    public static class LButtonElement
    {
        public static ElementType elementType = ElementType.Button;

        public static GameObject NewElement()
        {
            GameObject go = new GameObject("new_Button", typeof(Button));
            //var image = go.GetComponent<Image>();
            //image.raycastTarget = false;
            RectTransform rectTran = (RectTransform)go.transform;
            rectTran.sizeDelta = new Vector2(160, 30);
            go.transform.SetParent(LUIMgr.SecurityCheck().transform);
            go.transform.localPosition = new Vector3(0, 0, 0);
            LUIMgr.SetElementType(go, ElementType.Button);
            return go;
        }

        public static void SetProperty(GameObject go, string key, object prop)
        {

        }
    }
}