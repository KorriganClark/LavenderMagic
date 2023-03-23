using Lavender;
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
        MonsterCreater.Instance.Config = gameConfig.testMonsterConfig;
        MonsterCreater.Instance.defaultPos = gameConfig.defaultPosition;
    }

    // Update is called once per frame
    public static void Update()
    {
        var delta = Time.deltaTime;
        luaState.Update();

        InputMgr.Instance.Update();
        LCharacterControl.Instance.Update(delta);
        LEntityMgr.Instance.Update(delta);
        MonsterCreater.Instance.Update(delta);
    }
}
