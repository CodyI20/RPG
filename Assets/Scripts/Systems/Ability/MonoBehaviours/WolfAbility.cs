using UnityEngine;

[CreateAssetMenu(fileName = "WolfAbility", menuName = "ScriptableObjects/Abilities/WolfAbility", order = 0)]
public class WolfAbility : AbilityLogic
{
    public override void Execute()
    {
        EventBus<SpiritWolfSpawnedEvent>.Raise(new SpiritWolfSpawnedEvent());
        Debug.Log("Wolf Ability Executed");
    }
}
