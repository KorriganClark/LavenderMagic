using UnityEngine;
using UnityEngine.EventSystems;

namespace Lavender.UI
{
    public static class LUIMgr
    {
        public static GameObject UIRoot;
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
