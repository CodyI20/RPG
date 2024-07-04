using UnityEngine;

[CreateAssetMenu(fileName = "FlowerAbility", menuName = "ScriptableObjects/Abilities/FlowerAbility", order = 1)]
public class FlowerAbility : AbilityLogic
{
    public override void Execute()
    {
        EventBus<FlowerPowerEvent>.Raise(new FlowerPowerEvent());
        Debug.Log("Flower Ability Executed");
    }
}
