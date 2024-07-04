public class Ability
{
    public readonly AbilityData data;

    public Ability(AbilityData data)
    {
        this.data = data;
    }

    public AbilityCommand CreateCommand()
    {
        return new AbilityCommand(data);
    }
}
