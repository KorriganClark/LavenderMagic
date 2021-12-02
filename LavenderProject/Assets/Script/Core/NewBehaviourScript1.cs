using System.Collections;
using UnityEngine;

namespace Assets.Script.Core
{
    public class NewBehaviourScript1 : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            XLua.LuaEnv luaenv = new XLua.LuaEnv();
            luaenv.DoString("CS.UnityEngine.Debug.Log('hello world')");
            luaenv.Dispose();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}