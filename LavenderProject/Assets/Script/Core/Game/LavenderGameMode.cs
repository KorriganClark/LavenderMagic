using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LavenderGameMode
{
    public static readonly LuaClient luaState = new LuaClient();
    // Start is called before the first frame update
    public static void Start()
    {
        luaState.Init();

    }

    // Update is called once per frame
    public static void Update()
    {
        luaState.Update();
    }
}
