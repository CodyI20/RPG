using UnityEngine;
public class NPCHealthBar : HealthbarUpdate
{
    [SerializeField] private GameObject parentToCompareTo;
    EventBinding<NPCHealthChangeEvent> healthChangeEventBinding;

    private void OnEnable()
    {
        healthChangeEventBinding = new EventBinding<NPCHealthChangeEvent>(HandleHealthChangeEvent);
        EventBus<NPCHealthChangeEvent>.Register(healthChangeEventBinding);
    }

    private void OnDestroy()
    {
        EventBus<NPCHealthChangeEvent>.Deregister(healthChangeEventBinding);
    }

    private void HandleHealthChangeEvent(NPCHealthChangeEvent healthChangeEvent)
    {
        if (healthChangeEvent.npcObject == parentToCompareTo)
        {
            UpdateHealthBar(healthChangeEvent.currentHealthPercentage);
        }
    }
}
