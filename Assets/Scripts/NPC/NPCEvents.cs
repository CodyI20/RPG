using UnityEngine;
using System.Collections.Generic;
public struct NPCInteractInRangeEvent : IEvent
{
    public Transform selector;
    public Transform selection;
    public QuestGiver questGiver;
    public int questsCount;
    public List<QuestLogic> questsAvailable;
    public List<Quest> questsCompleted;
    public List<QuestLogic> questsInProgress;
    public float interactionRadius;
}

public struct NPCExitInteractionEvent : IEvent {
    public Transform selector;
    public Transform selection;
    public QuestGiver questGiver;
}

public struct NPCQuestAvailabilityEvent : IEvent
{
    public QuestGiver questGiver;
    public List<QuestLogic> questsAvailable;
    public List<Quest> questsCompleted;
    public List<QuestLogic> questsInProgress;
}

public struct NPCInteractOutOfRangeEvent : IEvent
{
    public Transform selector;
    public Transform selection;
    public QuestGiver questGiver;
    public float interactionRadius;
}

public struct NPCExitInteractionOutOfRangeEvent : IEvent
{
    public Transform selector;
    public Transform selection;
    public QuestGiver questGiver;
}