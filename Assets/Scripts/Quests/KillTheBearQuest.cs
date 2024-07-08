public class KillTheBearQuest : QuestLogic
{
    EventBinding<NPCDeathEvent> npcDeathEventBinding;

    private void OnEnable()
    {
        npcDeathEventBinding = new EventBinding<NPCDeathEvent>(HandleNPCDeath);
        EventBus<NPCDeathEvent>.Register(npcDeathEventBinding);
    }

    private void OnDestroy()
    {
        EventBus<NPCDeathEvent>.Deregister(npcDeathEventBinding);
    }

    private void HandleNPCDeath()
    {
        CompleteQuest(this);
    }
}
