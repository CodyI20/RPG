using UnityEngine;

[CreateAssetMenu(fileName = "HealAbility", menuName = "ScriptableObjects/Abilities/HealAbility", order = 0)]
public class HealEventRaiser : AbilityEventRaiser
{
    [SerializeField] private float _healAmount;
    public override void Execute()
    {
        EventBus<HealEvent>.Raise(new HealEvent() { amount = _healAmount});
    }
}
