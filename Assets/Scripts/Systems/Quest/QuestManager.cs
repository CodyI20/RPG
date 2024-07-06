using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class QuestManager : Singleton<QuestManager>
{
    [field: SerializeField] private List<QuestLogic> inProgressQuests = new List<QuestLogic>();

    private List<QuestLogic> spawnedQuestLogics = new List<QuestLogic>();

    EventBinding<QuestAcceptedEvent> QuestAcceptedEventBinding;
    EventBinding<QuestAbandonEvent> QuestAbandonEventBinding;

    private void OnEnable()
    {
        QuestAcceptedEventBinding = new EventBinding<QuestAcceptedEvent>(HandleQuestAccepted);
        EventBus<QuestAcceptedEvent>.Register(QuestAcceptedEventBinding);
        QuestAbandonEventBinding = new EventBinding<QuestAbandonEvent>(HandleQuestAbandoned);
        EventBus<QuestAbandonEvent>.Register(QuestAbandonEventBinding);
    }

    private void OnDisable()
    {
        EventBus<QuestAcceptedEvent>.Deregister(QuestAcceptedEventBinding);
        EventBus<QuestAbandonEvent>.Deregister(QuestAbandonEventBinding);
    }

    private void HandleQuestAccepted(QuestAcceptedEvent e)
    {
        AddQuest(e.questLogic);
#if UNITY_EDITOR
        Debug.Log("Quest accepted: " + e.questLogic);
#endif
    }

    private void HandleQuestAbandoned(QuestAbandonEvent e)
    {
        RemoveQuest(e.questLogic);
#if UNITY_EDITOR
        Debug.Log("Quest abandoned: " + e.questLogic);
#endif
    }

    private void AddQuest(QuestLogic questLogic)
    {
        this.inProgressQuests.Add(questLogic);
#if UNITY_EDITOR
        Debug.Log("Quest added: " + questLogic.quest.questName);
#endif
        QuestLogic spawnedQuestLogic = Instantiate(questLogic);
        spawnedQuestLogics.Add(spawnedQuestLogic);
    }

    private void RemoveQuest(QuestLogic questLogic)
    {
        this.inProgressQuests.Remove(questLogic);
        var foundQuestLogic = spawnedQuestLogics.Find(q => q.quest == questLogic.quest);

        if (foundQuestLogic != null)
        {
            spawnedQuestLogics.Remove(foundQuestLogic);
            Destroy(foundQuestLogic.gameObject);
        }
    }

    public void CompleteQuest(QuestLogic questLogic)
    {
        bool questFound = false;
        foreach(var quest in inProgressQuests)
        {
            if (quest.quest.questHash == questLogic.quest.questHash)
            {
                questFound = true;
            }
        }
        if(questFound == false)
        {
            return;
        }
        EventBus<QuestCompletedEvent>.Raise(new QuestCompletedEvent { questLogic = questLogic });
#if UNITY_EDITOR
        Debug.Log("Quest completed in the QuestManager: " + questLogic.quest.questName);
#endif
        RemoveQuest(questLogic);
    }

    public bool IsQuestCompleted(QuestLogic questLogic)
    {
        return !this.inProgressQuests.Contains(questLogic);
    }

    public bool IsQuestCompleted(string questName)
    {
        return inProgressQuests.Find(q => q.quest.name == questName) == null;
    }
}
