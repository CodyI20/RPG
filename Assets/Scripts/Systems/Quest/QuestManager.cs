using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class QuestManager : Singleton<QuestManager>
{
    private List<QuestLogic> questLogic = new List<QuestLogic>();

    EventBinding<QuestAcceptedEvent> QuestAcceptedEventBinding;

    private void OnEnable()
    {
        QuestAcceptedEventBinding = new EventBinding<QuestAcceptedEvent>(HandleQuestAccepted);
        EventBus<QuestAcceptedEvent>.Register(QuestAcceptedEventBinding);
    }

    private void OnDisable()
    {
        EventBus<QuestAcceptedEvent>.Deregister(QuestAcceptedEventBinding);
    }

    private void HandleQuestAccepted(QuestAcceptedEvent e)
    {
        AddQuest(e.questLogic);
#if UNITY_EDITOR
        Debug.Log("Quest accepted: " + e.questLogic);
#endif
    }

    public void AddQuest(QuestLogic questLogic)
    {
        this.questLogic.Add(questLogic);
#if UNITY_EDITOR
        Debug.Log("Quest added: " + questLogic.quest.questName);
#endif
#if UNITY_EDITOR
        Debug.Log("Quest added: " + questLogic.quest.questName);
#endif
        Instantiate(questLogic, transform);
    }

    public void CompleteQuest(QuestLogic questLogic)
    {
        EventBus<QuestCompletedEvent>.Raise(new QuestCompletedEvent { questLogic = questLogic });
#if UNITY_EDITOR
        Debug.Log("Quest completed in the QuestManager: " + questLogic.quest.questName);
#endif
        this.questLogic.Remove(questLogic);
        Destroy(questLogic.gameObject, 3f);// Adding a delay for debugging purposes
    }

    public bool IsQuestCompleted(QuestLogic questLogic)
    {
        return !this.questLogic.Contains(questLogic);
    }

    public bool IsQuestCompleted(string questName)
    {
        return questLogic.Find(q => q.quest.name == questName) == null;
    }
}
