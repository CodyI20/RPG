using UnityEngine;

[CreateAssetMenu(fileName = "SpiritWolfEventRaiser", menuName = "ScriptableObjects/Abilities/SpiritWolf", order = 1)]
public class SpiritWolfEventRaiser : AbilityEventRaiser
{
    public override void Execute()
    {
        EventBus<SpiritWolfSpawnedEvent>.Raise(new SpiritWolfSpawnedEvent());
    }
}
