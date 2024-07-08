using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class NPCQuestAvailabilityDisplay : MonoBehaviour
{
    [SerializeField] private QuestGiver questGiver;
    [SerializeField] private Sprite availableQuestMark;
    [SerializeField] private Sprite completeQuestMark;
    [SerializeField] private Sprite inProgressQuestMark;
    Image image;

    EventBinding<NPCQuestAvailabilityEvent> npcQuestAvailabilityBinding;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.preserveAspect = true;
    }

    private void OnEnable()
    {
        npcQuestAvailabilityBinding = new EventBinding<NPCQuestAvailabilityEvent>(HandleNPCQuestAvailability);
        EventBus<NPCQuestAvailabilityEvent>.Register(npcQuestAvailabilityBinding);
    }

    private void OnDisable()
    {
        EventBus<NPCQuestAvailabilityEvent>.Deregister(npcQuestAvailabilityBinding);
    }

    private void HandleNPCQuestAvailability(NPCQuestAvailabilityEvent e)
    {
        if (e.questGiver != questGiver)
        {
            return;
        }
        image.enabled = true;

        if (e.questsAvailable.Count > 0)
        {
            image.sprite = availableQuestMark;
        }else if(e.questsCompleted.Count > 0)
        {
            image.sprite = completeQuestMark;
        }else if(e.questsInProgress.Count > 0)
        {
            image.sprite = inProgressQuestMark;
        }
        else
        {
            image.enabled = false;
        }
    }
}
