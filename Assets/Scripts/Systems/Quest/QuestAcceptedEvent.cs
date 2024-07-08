public struct QuestAcceptedEvent : IEvent
{
    public QuestLogic questLogic;
}

public struct QuestDecliedEvent : IEvent
{
    public QuestLogic questLogic;
}

public struct QuestCompletedEvent : IEvent
{
    public QuestLogic questLogic;
}

public struct QuestPreviewEvent : IEvent
{
    public QuestLogic questLogic;
}

public struct QuestInProgressPreviewEvent : IEvent
{
    public QuestLogic questLogic;
}

public struct QuestAbandonEvent : IEvent
{
    public QuestLogic questLogic;
}

public struct QuestTurnedInEvent : IEvent
{
    public QuestLogic questLogic;
}

public struct QuestPreviewExitEvent : IEvent
{
}
