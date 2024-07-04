using UnityEngine;

public class SpiritWolfEventHandler : MonoBehaviour
{
    EventBinding<SpiritWolfSpawnedEvent> spiritWolfSpawnedEventBinding;
    EventBinding<SpiritWolfSpawnFailedEvent> spiritWolfSpawnFailedEventBinding;

    private void OnEnable()
    {
        spiritWolfSpawnedEventBinding = new EventBinding<SpiritWolfSpawnedEvent>(OnSpiritWolfSpawned);
        EventBus<SpiritWolfSpawnedEvent>.Register(spiritWolfSpawnedEventBinding);
        spiritWolfSpawnFailedEventBinding = new EventBinding<SpiritWolfSpawnFailedEvent>(OnSpiritWolfSpawnFailed);
        EventBus<SpiritWolfSpawnFailedEvent>.Register(spiritWolfSpawnFailedEventBinding);
    }

    private void OnDisable()
    {
        EventBus<SpiritWolfSpawnedEvent>.Deregister(spiritWolfSpawnedEventBinding);
        EventBus<SpiritWolfSpawnFailedEvent>.Deregister(spiritWolfSpawnFailedEventBinding);
    }

    private void OnSpiritWolfSpawned(SpiritWolfSpawnedEvent e)
    {
        Debug.Log("Spirit Wolf has been spawned successfully.");
        // Handle the event, e.g., update UI
    }

    private void OnSpiritWolfSpawnFailed(SpiritWolfSpawnFailedEvent e)
    {
        Debug.Log($"Failed to spawn Spirit Wolf. Cooldown remaining: {e.cooldownRemaining:F1} seconds.");
        // Handle the event, e.g., show a cooldown timer on the UI
    }
}
