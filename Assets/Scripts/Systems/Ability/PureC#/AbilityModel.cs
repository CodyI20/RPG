public class AbilityModel
{
    public readonly ObservableList<Ability> abilities = new();

    public void Add(Ability a)
    {
        abilities.Add(a);
    }
}
