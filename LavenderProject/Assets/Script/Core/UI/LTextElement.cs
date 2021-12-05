﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Lavender.UI
{
    public static class LTextElement 
    {
        public static ElementType elementType = ElementType.Text;

        public static GameObject NewElement()
        {
            GameObject go = new GameObject("new_Text", typeof(Text));
            var text = go.GetComponent<Text>();
            text.raycastTarget = false;
            text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");//AssetDatabase.LoadAssetAtPath<Font>("Assets/Arts/Fonts/zh_cn.TTF");   // 默认字体
            text.text = "New Text";
            go.transform.SetParent(LUIMgr.SecurityCheck().transform);
            go.transform.localPosition = new Vector3(0, 0, 0);
            
            return go;
        }
    }
}
