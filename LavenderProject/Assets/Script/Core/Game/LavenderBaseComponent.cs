using Lavender.UnityFramework;
using UnityEditor;

namespace Lavender{
    class LavenderBaseComponent : BaseComponent
{
    private void Start()
    {
        LavenderGameMode.gameConfig = gameObject.GetComponent<GameConfig>();
        LavenderGameMode.Start();
    }

    protected override void Update()
    {
        base.Update();
        LavenderGameMode.Update();
    }
}
}

