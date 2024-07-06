using UnityEngine;
public struct NPCInteractInRangeEvent : IEvent
{
    public Transform selector;
    public Transform selection;
    public QuestGiver questGiver;
    public int questsCount;
    public float interactionRadius;
}