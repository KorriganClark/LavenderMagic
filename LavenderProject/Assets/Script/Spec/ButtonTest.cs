using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonTest : MonoBehaviour
{
    [SerializeField]
    public Font font;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        LavenderGameMode.luaState.Excute("UIMgr:ActiveUI()");
        /*
        var canvasObj = SecurityCheck();

        if (!Selection.activeTransform)      // 在根目录创建的， 自动移动到 Canvas下
        {
            // Debug.Log("没有选择对象");
            NewText("notActiveTran").transform.SetParent(canvasObj.transform);
        }
        else // (Selection.activeTransform)
        {
            if (!Selection.activeTransform.GetComponentInParent<Canvas>())    // 没有在UI树下
            {
                NewText("notActiveTranCanvas").transform.SetParent(canvasObj.transform);
            }
            else
            {
                NewText("ActiveTranCanvas");
            }
        }*/
        //print("test!!");
    }

    private GameObject NewText(string txt)
    {
        GameObject go = new GameObject("x_Text", typeof(Text));
        var text = go.GetComponent<Text>();
        text.raycastTarget = false;
        text.font = font;// AssetDatabase.LoadAssetAtPath<Font>("Assets/Arts/Fonts/zh_cn.TTF");   // 默认字体
        text.text = txt;
        go.transform.SetParent(Selection.activeTransform);
        Selection.activeGameObject = go;

        //go.AddComponent<Outline>();   // 默认添加 附加组件
        return go;
    }
    private GameObject SecurityCheck()
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
