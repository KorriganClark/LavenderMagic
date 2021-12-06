using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Lavender.UI
{
    public static class LUIMgr
    {
        public static GameObject UIRoot;

        private static Dictionary<GameObject, ElementType> instanceElementType = new Dictionary<GameObject, ElementType>();
        public static ElementType GetElementType(GameObject instance)
        {
            ElementType res;
            if(instanceElementType.TryGetValue(instance, out res))
            {
                return res;
            }
            return ElementType.Null;
        }

        public static void SetElementType(GameObject instance, ElementType type)
        {
            instanceElementType.Add(instance, type);
        }

        public static GameObject GetUIRoot()
        {
            if(UIRoot == null)
            {
                UIRoot = new GameObject("UIRoot");
                UIRoot.transform.SetParent(SecurityCheck().transform);
                UIRoot.transform.localPosition = new Vector3(0, 0, 0);
            }
            return UIRoot;
        }

        public static GameObject SecurityCheck()
        {
            GameObject canvas;
            var cc = Object.FindObjectOfType<Canvas>();
            if (!cc)
            {
                canvas = new GameObject("_Canvas", typeof(Canvas));
            }
            else
            {
                canvas = cc.gameObject;
            }
            if (!Object.FindObjectOfType<EventSystem>())
            {
                GameObject eventSystem = new GameObject("_EventSystem", typeof(EventSystem));
            }

            return canvas;
        }
    }
}
