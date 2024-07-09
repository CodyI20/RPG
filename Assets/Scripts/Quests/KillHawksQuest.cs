using UnityEngine;
public class KillHawksQuest : QuestLogic
{
    [SerializeField] private int _hawksToKill = 5;
    private int _hawksKilled = 0;

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

    private void HandleNPCDeath(NPCDeathEvent e)
    {
        if (e.npcObject.CompareTag("HawkHostileNPC"))
        {
            _hawksKilled++;
        }
    }

    private void Update()
    {
        if(_hawksKilled >= _hawksToKill)
        {
#if UNITY_EDITOR
            Debug.Log($"Quest {this} completed!");
#endif
            CompleteQuest(this);
        }
    }

}
