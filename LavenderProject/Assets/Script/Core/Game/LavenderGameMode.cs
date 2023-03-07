using Assets.Script.Core.Entity;
using Lavender;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LavenderGameMode
{
    public static readonly LuaClient luaState = new LuaClient();
    public static GameConfig gameConfig;

    public static void Start()
    {
        luaState.Init();
        var chara = LEntityMgr.Instance.CreateEntity<LCharacter>(gameConfig.defaultCharacterConfig);
        chara.Root.transform.position = gameConfig.defaultPosition;
        LCharacterControl.Instance.SetTarget(chara);

    }

    // Update is called once per frame
    public static void Update()
    {
        var delta = Time.deltaTime;
        luaState.Update();
        LCharacterControl.Instance.Update(delta);
        LEntityMgr.Instance.Update(delta);
        InputMgr.Instance.Update();
    }
}
