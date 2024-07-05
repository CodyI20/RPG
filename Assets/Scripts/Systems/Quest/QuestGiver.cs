using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] Quest[] quests;
    private Queue<Quest> questQueue = new Queue<Quest>();

    [Header("Quest Giver Settings")]
    [SerializeField] private float questGiverInteractionRadius = 1f;

    EventBinding<QuestAcceptedEvent> QuestAcceptedEventBinding;

    private void Awake()
    {
        foreach (Quest quest in quests)
        {
            questQueue.Enqueue(quest);
        }
    }

    private void OnEnable()
    {
        QuestAcceptedEventBinding = new EventBinding<QuestAcceptedEvent>(HandleQuestAccepted);
        EventBus<QuestAcceptedEvent>.Register(QuestAcceptedEventBinding);
        ObjectSelector.OnSelection += HandleSelection;
    }
    private void OnDisable()
    {
        EventBus<QuestAcceptedEvent>.Deregister(QuestAcceptedEventBinding);
        ObjectSelector.OnSelection -= HandleSelection;
    }

    private void HandleQuestAccepted(QuestAcceptedEvent e)
    {
        if (questQueue.Count > 0)
        {
#if UNITY_EDITOR
            Debug.Log("Quest accepted: " + questQueue.Peek().questName + "; Dequeueing...");
#endif
            questQueue.Dequeue();
        }
    }

    private void HandleSelection(Transform selector, Transform selection)
    {
        if(questQueue.Count == 0) return;
        if (selection == transform)
        {
            if (Vector3.Distance(selector.position, selection.position) <= questGiverInteractionRadius)
            {
#if UNITY_EDITOR
                Debug.Log("Quest giver selected and in range!");
#endif
                EventBus<QuestPreviewEvent>.Raise(new QuestPreviewEvent { quest = questQueue.Peek() });
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
