public struct SpiritWolfSpawnedEvent : IEvent
{
}

public struct SpiritWolfSpawnFailedEvent : IEvent
{
    public float cooldownRemaining;
}

public struct FeignDeathEvent : IEvent
{

}

public struct HealEvent : IEvent
{
    public float amount;
}
