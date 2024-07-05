using UnityEngine;

public abstract class QuestLogic : MonoBehaviour
{
    [field: SerializeField] public Quest quest { get; private set; }

    protected void AddQuest()
    {
        QuestManager.Instance.AddQuest(this);
    }
    protected void CompleteQuest()
    {
#if UNITY_EDITOR
        Debug.Log("Quest completed: " + quest.questName);
#endif
        QuestManager.Instance.CompleteQuest(this);
    }
}
