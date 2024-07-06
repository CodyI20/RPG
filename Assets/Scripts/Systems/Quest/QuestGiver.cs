using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] QuestLogic[] quests;
    private Queue<QuestLogic> questQueue = new Queue<QuestLogic>();
    private Queue<QuestLogic> finishedQuestsQueue = new Queue<QuestLogic>();

    [Header("Quest Giver Settings")]
    [SerializeField] private float questGiverInteractionRadius = 1f;

    EventBinding<QuestAcceptedEvent> QuestAcceptedEventBinding;
    EventBinding<QuestAbandonEvent> QuestAbandonEventBinding;

    private void Awake()
    {
        foreach (QuestLogic quest in quests)
        {
            questQueue.Enqueue(quest);
        }
    }

    public QuestLogic[] questLogics => quests;

    private void OnEnable()
    {
        QuestAcceptedEventBinding = new EventBinding<QuestAcceptedEvent>(HandleQuestAccepted);
        EventBus<QuestAcceptedEvent>.Register(QuestAcceptedEventBinding);

        QuestAbandonEventBinding = new EventBinding<QuestAbandonEvent>(HandleQuestAbandoned);
        EventBus<QuestAbandonEvent>.Register(QuestAbandonEventBinding);
        ObjectSelector.OnSelection += HandleSelection;
    }
    private void OnDisable()
    {
        EventBus<QuestAcceptedEvent>.Deregister(QuestAcceptedEventBinding);
        EventBus<QuestAbandonEvent>.Deregister(QuestAbandonEventBinding);
        ObjectSelector.OnSelection -= HandleSelection;
    }

    private void HandleQuestAccepted(QuestAcceptedEvent e)
    {
        if (questQueue.Count > 0)
        {
#if UNITY_EDITOR
            Debug.Log("Quest accepted: " + questQueue.Peek().quest.questName + "; Dequeueing...");
#endif
            questQueue.Dequeue();
        }
    }

    private void HandleQuestAbandoned(QuestAbandonEvent e)
    {
#if UNITY_EDITOR
        Debug.Log("Abandoning quest: " + e.questLogic.quest.questName + "; Enqueueing...");
#endif
        questQueue.Enqueue(e.questLogic);
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
                EventBus<NPCInteractInRangeEvent>.Raise(new NPCInteractInRangeEvent { selector = selector, selection = selection, questGiver = this, questsCount = questQueue.Count }) ;
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
