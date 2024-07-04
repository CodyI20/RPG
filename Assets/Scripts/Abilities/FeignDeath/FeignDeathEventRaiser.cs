using UnityEngine;

[CreateAssetMenu(fileName = "FeignDeath", menuName = "ScriptableObjects/Abilities/FeignDeath", order = 1)]
public class FeignDeathEventRaiser : AbilityEventRaiser
{
    public override void Execute()
    {
        EventBus<FeignDeathEvent>.Raise(new FeignDeathEvent());
    }
}
