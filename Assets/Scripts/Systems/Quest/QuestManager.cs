using System.Collections.Generic;
using UnityUtils;

public class QuestManager : PersistentSingleton<QuestManager>
{
    private List<QuestLogic> quests = new List<QuestLogic>();

    public void AddQuest(QuestLogic quest)
    {
        quests.Add(quest);
    }

    public void CompleteQuest(QuestLogic quest)
    {
        quests.Remove(quest);
        Destroy(quest.gameObject);
    }

    public bool IsQuestCompleted(QuestLogic quest)
    {
        return !quests.Contains(quest);
    }

    public bool IsQuestCompleted(string questName)
    {
        return quests.Find(q => q.quest.questName == questName) == null;
    }
}
