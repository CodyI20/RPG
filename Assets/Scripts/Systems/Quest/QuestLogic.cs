using UnityEngine;

public abstract class QuestLogic : MonoBehaviour
{
    [field: SerializeField] public Quest quest { get; private set; }

    protected void CompleteQuest()
    {
#if UNITY_EDITOR
        Debug.Log("Quest completed in QuestLogic: " + quest.questName);
#endif
        QuestManager.Instance.CompleteQuest(this);
    }
}
