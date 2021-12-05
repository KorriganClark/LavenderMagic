using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavenderGameMode : MonoBehaviour
{

    public static LuaClient luaState = new LuaClient();
    // Start is called before the first frame update
    void Start()
    {
        luaState.Init();
    }

    // Update is called once per frame
    void Update()
    {
        luaState.Update();
    }
}
