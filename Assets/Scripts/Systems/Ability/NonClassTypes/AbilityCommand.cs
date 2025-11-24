public class AbilityCommand : ICommand
{
    public readonly AbilityData data;
    public float duration => data.castTime;

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
        data.eventRaiser.Execute();
    }
}
