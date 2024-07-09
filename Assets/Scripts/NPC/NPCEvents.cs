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

public struct NPCDeathEvent : IEvent
{
    public GameObject npcObject;
}

public struct NPCEvadeFinishedEvent : IEvent
{
    public GameObject npcObject;
}

public struct NPCAttackEvent : IEvent
{
    public GameObject npcObject;
}

public struct NPCWalkEvent : IEvent
{
    public GameObject npcObject;
}

public struct NPCRunEvent : IEvent
{
    public GameObject npcObject;
}

public struct NPCIdleEvent : IEvent
{
    public GameObject npcObject;
}

public struct NPCEvasionEvent : IEvent
{
    public GameObject npcObject;
}

public struct NPCTriggerCombatEvent : IEvent
{
    public GameObject npcObject;
}

public struct NPCHealthChangeEvent : IEvent
{
    public GameObject npcObject;
    public float currentHealthPercentage;
}