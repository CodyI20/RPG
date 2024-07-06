using UnityEngine;

public abstract class QuestLogic : MonoBehaviour
{
    [field: SerializeField] public Quest quest { get; private set; }

    protected void CompleteQuest(QuestLogic quest)
    {
        QuestManager.Instance.CompleteQuest(quest);
    }
}
