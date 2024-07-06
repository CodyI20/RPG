using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] QuestLogic[] quests;
    private Queue<QuestLogic> availableQuestsQueue = new Queue<QuestLogic>();
    private List<QuestLogic> inProgressQuestsList = new List<QuestLogic>();
    private List<QuestLogic> finishedQuestsList = new List<QuestLogic>();

    [Header("Quest Giver Settings")]
    [SerializeField] private float questGiverInteractionRadius = 1f;

    EventBinding<QuestAcceptedEvent> QuestAcceptedEventBinding;
    EventBinding<QuestAbandonEvent> QuestAbandonEventBinding;
    EventBinding<QuestCompletedEvent> QuestCompletedEventBinding;

    private void Awake()
    {
        foreach (QuestLogic quest in quests)
        {
            availableQuestsQueue.Enqueue(quest);
        }
    }

    public QuestLogic[] questLogics => quests;

    private void OnEnable()
    {
        QuestAcceptedEventBinding = new EventBinding<QuestAcceptedEvent>(HandleQuestAccepted);
        EventBus<QuestAcceptedEvent>.Register(QuestAcceptedEventBinding);

        QuestAbandonEventBinding = new EventBinding<QuestAbandonEvent>(HandleQuestAbandoned);
        EventBus<QuestAbandonEvent>.Register(QuestAbandonEventBinding);

        QuestCompletedEventBinding = new EventBinding<QuestCompletedEvent>(HandleQuestCompleted);
        EventBus<QuestCompletedEvent>.Register(QuestCompletedEventBinding);
        ObjectSelector.OnSelection += HandleSelection;
    }
    private void OnDisable()
    {
        EventBus<QuestAcceptedEvent>.Deregister(QuestAcceptedEventBinding);
        EventBus<QuestAbandonEvent>.Deregister(QuestAbandonEventBinding);
        EventBus<QuestCompletedEvent>.Deregister(QuestCompletedEventBinding);
        ObjectSelector.OnSelection -= HandleSelection;
    }

    private void HandleQuestAccepted(QuestAcceptedEvent e)
    {
        if (availableQuestsQueue.Count > 0)
        {
#if UNITY_EDITOR
            Debug.Log("Quest accepted: " + availableQuestsQueue.Peek().quest.questName + "; Dequeueing...");
#endif
            availableQuestsQueue.Dequeue();
            inProgressQuestsList.Add(e.questLogic);
        }
    }

    private void HandleQuestAbandoned(QuestAbandonEvent e)
    {
#if UNITY_EDITOR
        Debug.Log("Abandoning quest: " + e.questLogic.quest.questName + "; Enqueueing...");
#endif
        availableQuestsQueue.Enqueue(e.questLogic);
        inProgressQuestsList.Remove(e.questLogic);
    }

    private void HandleQuestCompleted(QuestCompletedEvent e)
    {
#if UNITY_EDITOR
        Debug.Log("Quest completed: " + e.questLogic.quest.questName + "; Enqueueing...");
#endif
        finishedQuestsList.Add(e.questLogic);
        inProgressQuestsList.Remove(e.questLogic);
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
                EventBus<NPCInteractInRangeEvent>.Raise(new NPCInteractInRangeEvent { 
                    selector = selector, 
                    selection = selection, 
                    questGiver = this, 
                    questsCount = availableQuestsQueue.Count + finishedQuestsList.Count + inProgressQuestsList.Count, 
                    questsAvailable = availableQuestsQueue.ToList(),
                    questsCompleted = finishedQuestsList,
                    questsInProgress = inProgressQuestsList
                });
            }
            else
            {
#if UNITY_EDITOR
                EventBus<OutOfRangeQuestGrabEvent>.Raise(new OutOfRangeQuestGrabEvent());
                Debug.Log("Quest giver selected but out of range!");
#endif
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, questGiverInteractionRadius);
    }
}
