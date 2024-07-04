public class AbilityCommand : ICommand
{
    private readonly AbilityData data;
    public float duration => data.cooldown;

    public AbilityCommand(AbilityData data)
    {
        this.data = data;
    }

    public void Execute()
    {
        EventBus<PlayerAnimationEvent>.Raise(new PlayerAnimationEvent
        {
            animationHash = data.animationHash
        });
        EventBus<SpiritWolfSpawnedEvent>.Raise(new SpiritWolfSpawnedEvent());
    }
}
