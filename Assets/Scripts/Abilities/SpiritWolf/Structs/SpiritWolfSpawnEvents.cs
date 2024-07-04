public struct SpiritWolfSpawnedEvent : IEvent
{
}

public struct SpiritWolfSpawnFailedEvent : IEvent
{
    public float cooldownRemaining;
}
