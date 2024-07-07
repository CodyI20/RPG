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