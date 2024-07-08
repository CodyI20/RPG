using UnityEngine;

public class QuestSoundManager : SoundManager
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip questTurnInSound;
    [SerializeField] private AudioClip questAbandonSound;
    [SerializeField] private AudioClip questAcceptSound;
    [SerializeField] private AudioClip questPreviewExitSound;
    [SerializeField] private AudioClip questPreviewEnterSound;

    //Event bus bindings
    EventBinding<QuestTurnedInEvent> questTurnedInBinding;
    EventBinding<QuestAbandonEvent> questAbandonedBinding;
    EventBinding<QuestAcceptedEvent> questAcceptedBinding;
    EventBinding<QuestPreviewEvent> questPreviewBinding;
    EventBinding<QuestInProgressPreviewEvent> questInProgressPreviewBinding;
    EventBinding<QuestPreviewExitEvent> questPreviewExitBinding;

    private void OnEnable()
    {
        questTurnedInBinding = new EventBinding<QuestTurnedInEvent>(HandleQuestTurnedIn);
        EventBus<QuestTurnedInEvent>.Register(questTurnedInBinding);

        questAbandonedBinding = new EventBinding<QuestAbandonEvent>(HandleQuestAbandoned);
        EventBus<QuestAbandonEvent>.Register(questAbandonedBinding);

        questAcceptedBinding = new EventBinding<QuestAcceptedEvent>(HandleQuestAccepted);
        EventBus<QuestAcceptedEvent>.Register(questAcceptedBinding);

        questPreviewBinding = new EventBinding<QuestPreviewEvent>(HandleQuestPreview);
        EventBus<QuestPreviewEvent>.Register(questPreviewBinding);

        questInProgressPreviewBinding = new EventBinding<QuestInProgressPreviewEvent>(HandleQuestInProgressPreview);
        EventBus<QuestInProgressPreviewEvent>.Register(questInProgressPreviewBinding);

        questPreviewExitBinding = new EventBinding<QuestPreviewExitEvent>(HandleQuestPreviewExit);
        EventBus<QuestPreviewExitEvent>.Register(questPreviewExitBinding);
    }

    private void OnDisable()
    {
        EventBus<QuestTurnedInEvent>.Deregister(questTurnedInBinding);
        EventBus<QuestAbandonEvent>.Deregister(questAbandonedBinding);
        EventBus<QuestAcceptedEvent>.Deregister(questAcceptedBinding);
        EventBus<QuestPreviewEvent>.Deregister(questPreviewBinding);
        EventBus<QuestInProgressPreviewEvent>.Deregister(questInProgressPreviewBinding);
        EventBus<QuestPreviewExitEvent>.Deregister(questPreviewExitBinding);
    }

    private void HandleQuestTurnedIn(QuestTurnedInEvent e)
    {
        PlayClip(questTurnInSound);
    }

    private void HandleQuestAbandoned(QuestAbandonEvent e)
    {
        PlayClip(questAbandonSound);
    }

    private void HandleQuestAccepted(QuestAcceptedEvent e)
    {
        PlayClip(questAcceptSound);
    }

    private void HandleQuestPreview(QuestPreviewEvent e)
    {
        PlayClip(questPreviewEnterSound);
    }

    private void HandleQuestInProgressPreview(QuestInProgressPreviewEvent e)
    {
        PlayClip(questPreviewEnterSound);
    }

    private void HandleQuestPreviewExit(QuestPreviewExitEvent e)
    {
        PlayClip(questPreviewExitSound);
    }
}
