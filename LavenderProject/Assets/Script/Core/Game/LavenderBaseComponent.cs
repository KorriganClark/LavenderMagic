using Lavender.UnityFramework;

class LavenderBaseComponent : BaseComponent
{
    private void Start()
    {
        LavenderGameMode.Start();
    }

    protected override void Update()
    {
        base.Update();
        LavenderGameMode.Update();
    }
}
