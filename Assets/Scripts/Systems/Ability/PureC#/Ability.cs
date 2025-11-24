using AbilityUtilities;

public class Ability
{
    public readonly AbilityData data;
    public readonly CountdownTimer CooldownTimer;

    public Ability(AbilityData data)
    {
        this.data = data;
        CooldownTimer = new CountdownTimer(data.cooldown);
    }

    public AbilityCommand CreateCommand()
    {
        return new AbilityCommand(data);
    }

    public void Tick(float deltaTime)
    {
        CooldownTimer.Tick(deltaTime);
    }

    public void StartCooldown()
    {
        CooldownTimer.Reset(data.cooldown);
        CooldownTimer.Start();
    }

    public bool IsReady => !CooldownTimer.IsRunning;
}
