public struct QuestEvent : IEvent
{
    public Quest quest;
    public QuestState questState;
}

public struct OutOfRangeQuestGrabEvent : IEvent
{
}
