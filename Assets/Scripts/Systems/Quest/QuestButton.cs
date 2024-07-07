using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuestButton : MonoBehaviour
{
    [HideInInspector] public QuestLogic questLogic;
    private Button button;

    EventBinding<QuestAbandonEvent> abandonEvent;
    EventBinding<QuestTurnedInEvent> turnInEvent;

    private void Awake()
    {
        turnInEvent = new EventBinding<QuestTurnedInEvent>(HandleQuestTurnedIn);
        EventBus<QuestTurnedInEvent>.Register(turnInEvent);
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        abandonEvent = new EventBinding<QuestAbandonEvent>(HandleQuestAbandon);
        EventBus<QuestAbandonEvent>.Register(abandonEvent);
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnDestroy()
    {
        EventBus<QuestAbandonEvent>.Deregister(abandonEvent);
        EventBus<QuestTurnedInEvent>.Deregister(turnInEvent);
    }

    private void HandleQuestAbandon(QuestAbandonEvent e)
    {
        if (e.questLogic == questLogic)
        {
            Destroy(gameObject);
        }
    }

    private void HandleQuestTurnedIn(QuestTurnedInEvent e)
    {
#if UNITY_EDITOR
        Debug.Log("QUEST BUTTON: Quest turned in: " + e.questLogic.quest.questName);
#endif
        if (e.questLogic.quest.questHash == questLogic.quest.questHash)
        {
            Destroy(gameObject);
        }
    }

    private void OnButtonClicked()
    {
        EventBus<QuestPreviewEvent>.Raise(new QuestPreviewEvent { questLogic = this.questLogic });
    }

}
