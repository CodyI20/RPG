public struct QuestAcceptedEvent : IEvent
{
    public Quest quest;
}

public struct QuestDecliedEvent : IEvent
{
    public Quest quest;
}

public struct QuestCompletedEvent : IEvent
{
    public Quest quest;
}

public struct OutOfRangeQuestGrabEvent : IEvent
{
}

public struct QuestPreviewEvent : IEvent
{
    public Quest quest;
}
