using UnityEngine;

public class WarningMessagesSoundManager : SoundManager
{
    [Header("Warning message sounds")]
    [SerializeField] private AudioClip[] OutOfRangeSounds;

    EventBinding<NPCInteractOutOfRangeEvent> warningInteractOutOfRangeEvent;

    private void OnEnable()
    {
        warningInteractOutOfRangeEvent = new EventBinding<NPCInteractOutOfRangeEvent>(HandleInteractOutOfRange);
        EventBus<NPCInteractOutOfRangeEvent>.Register(warningInteractOutOfRangeEvent);
    }

    private void OnDisable()
    {
        EventBus<NPCInteractOutOfRangeEvent>.Deregister(warningInteractOutOfRangeEvent);
    }

    private void HandleInteractOutOfRange(NPCInteractOutOfRangeEvent e)
    {
        PlayClip(PickRandomAudioClip(OutOfRangeSounds));
    }
}
