using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    // In the future make it so that <quests> holds all of the quests from this specific quest giver;
    // <availableQuestsList> holds all of the quests that the player is currently eligible for;
    // Instead of adding them all in Awake, create logic for eligibility checking and add them to <availableQuestsList> accordingly;

    [SerializeField] List<QuestLogic> quests;
    private List<QuestLogic> availableQuestsList = new List<QuestLogic>();
    private List<QuestLogic> inProgressQuestsList = new List<QuestLogic>();
    private List<Quest> finishedQuestsList = new List<Quest>();

    private void Awake()
    {
        availableQuestsList.AddRange(quests);
        EventBus<NPCQuestAvailabilityEvent>.Raise(new NPCQuestAvailabilityEvent
        {
            questGiver = this,
            questsAvailable = availableQuestsList,
            questsCompleted = finishedQuestsList,
            questsInProgress = inProgressQuestsList
        });
    }

    [Header("Quest Giver Settings")]
    [SerializeField] private float questGiverInteractionRadius = 1f;

    EventBinding<QuestAcceptedEvent> QuestAcceptedEventBinding;
    EventBinding<QuestAbandonEvent> QuestAbandonEventBinding;
    EventBinding<QuestCompletedEvent> QuestCompletedEventBinding;
    EventBinding<QuestTurnedInEvent> QuestTurnedInEventBinding;


    public List<QuestLogic> questLogics => quests;

    private void OnEnable()
    {
        QuestAcceptedEventBinding = new EventBinding<QuestAcceptedEvent>(HandleQuestAccepted);
        EventBus<QuestAcceptedEvent>.Register(QuestAcceptedEventBinding);

        QuestAbandonEventBinding = new EventBinding<QuestAbandonEvent>(HandleQuestAbandoned);
        EventBus<QuestAbandonEvent>.Register(QuestAbandonEventBinding);

        QuestCompletedEventBinding = new EventBinding<QuestCompletedEvent>(HandleQuestCompleted);
        EventBus<QuestCompletedEvent>.Register(QuestCompletedEventBinding);

        QuestTurnedInEventBinding = new EventBinding<QuestTurnedInEvent>(HandleQuestTurnedIn);
        EventBus<QuestTurnedInEvent>.Register(QuestTurnedInEventBinding);
        ObjectSelector.OnSelection += HandleSelection;
        ObjectSelector.OnDeselection += HandleDeselection;
    }
    private void OnDisable()
    {
        EventBus<QuestAcceptedEvent>.Deregister(QuestAcceptedEventBinding);
        EventBus<QuestAbandonEvent>.Deregister(QuestAbandonEventBinding);
        EventBus<QuestCompletedEvent>.Deregister(QuestCompletedEventBinding);
        EventBus<QuestTurnedInEvent>.Deregister(QuestTurnedInEventBinding);
        ObjectSelector.OnSelection -= HandleSelection;
        ObjectSelector.OnDeselection -= HandleDeselection;
    }

    private void HandleQuestAccepted(QuestAcceptedEvent e)
    {
        availableQuestsList.Remove(e.questLogic);
        inProgressQuestsList.Add(e.questLogic);
        EventBus<NPCQuestAvailabilityEvent>.Raise(new NPCQuestAvailabilityEvent
        {
            questGiver = this,
            questsAvailable = availableQuestsList,
            questsCompleted = finishedQuestsList,
            questsInProgress = inProgressQuestsList
        });
    }

    private void HandleQuestAbandoned(QuestAbandonEvent e)
    {
#if UNITY_EDITOR
        Debug.Log("Abandoning quest: " + e.questLogic.quest.questName + "; Enqueueing...");
#endif
        availableQuestsList.Add(e.questLogic);
        inProgressQuestsList.Remove(e.questLogic);
        EventBus<NPCQuestAvailabilityEvent>.Raise(new NPCQuestAvailabilityEvent
        {
            questGiver = this,
            questsAvailable = availableQuestsList,
            questsCompleted = finishedQuestsList,
            questsInProgress = inProgressQuestsList
        });
    }

    private void HandleQuestCompleted(QuestCompletedEvent e)
    {
#if UNITY_EDITOR
        Debug.Log("Quest completed: " + e.questLogic.quest.questName + "; Enqueueing...");
#endif
        finishedQuestsList.Add(e.questLogic.quest);
        inProgressQuestsList.Remove(e.questLogic);
        EventBus<NPCQuestAvailabilityEvent>.Raise(new NPCQuestAvailabilityEvent
        {
            questGiver = this,
            questsAvailable = availableQuestsList,
            questsCompleted = finishedQuestsList,
            questsInProgress = inProgressQuestsList
        });
    }

    private void HandleSelection(Transform selector, Transform selection)
    {
        if (selection == transform)
        {
            if (Vector3.Distance(selector.position, selection.position) <= questGiverInteractionRadius)
            {
#if UNITY_EDITOR
                Debug.Log("Quest giver selected and in range!");
#endif
                EventBus<NPCInteractInRangeEvent>.Raise(new NPCInteractInRangeEvent
                {
                    selector = selector,
                    selection = selection,
                    questGiver = this,
                    questsCount = availableQuestsList.Count + finishedQuestsList.Count + inProgressQuestsList.Count,
                    questsAvailable = availableQuestsList,
                    questsCompleted = finishedQuestsList,
                    questsInProgress = inProgressQuestsList,
                    interactionRadius = questGiverInteractionRadius
                });
            }
            else
            {
#if UNITY_EDITOR
                EventBus<NPCInteractOutOfRangeEvent>.Raise(new NPCInteractOutOfRangeEvent()
                {
                    questGiver = this,
                    interactionRadius = questGiverInteractionRadius,
                    selection = selection,
                    selector = selector
                });
                Debug.Log("Quest giver selected but out of range!");
#endif
            }
        }
    }

    private void HandleDeselection(Transform selector, Transform deselection)
    {
#if UNITY_EDITOR
        Debug.Log("Deselected: " + deselection.name);
#endif
    }

    private void HandleQuestTurnedIn(QuestTurnedInEvent e)
    {
        finishedQuestsList.Remove(e.questLogic.quest);
        inProgressQuestsList.Remove(e.questLogic);
        quests.Remove(e.questLogic);
#if UNITY_EDITOR
        Debug.Log($"Quests left: {quests.Count}; finishedQuests: {finishedQuestsList.Count}; inProgressQuests: {inProgressQuestsList.Count}; availableQuests: {availableQuestsList.Count}");
#endif
        EventBus<NPCQuestAvailabilityEvent>.Raise(new NPCQuestAvailabilityEvent
        {
            questGiver = this,
            questsAvailable = availableQuestsList,
            questsCompleted = finishedQuestsList,
            questsInProgress = inProgressQuestsList
        });
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, questGiverInteractionRadius);
    }
}
