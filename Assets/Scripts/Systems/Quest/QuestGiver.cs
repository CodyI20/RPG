using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] Quest[] quests;
    private Queue<Quest> questQueue = new Queue<Quest>();

    [Header("Quest Giver Settings")]
    [SerializeField] private float questGiverInteractionRadius = 1f;

    private void Awake()
    {
        foreach (Quest quest in quests)
        {
            questQueue.Enqueue(quest);
        }
    }

    private void OnEnable()
    {
        ObjectSelector.OnSelection += HandleSelection;
    }
    private void OnDisable()
    {
        ObjectSelector.OnSelection -= HandleSelection;
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
