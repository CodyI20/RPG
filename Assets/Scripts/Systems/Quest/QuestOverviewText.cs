using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(Button))]
public class QuestOverviewText : MonoBehaviour
{
    [HideInInspector] public QuestLogic questLogic;
    private TextMeshProUGUI text;
    private Button button;
    EventBinding<QuestCompletedEvent> questCompletedBinding;

    EventBinding<QuestAbandonEvent> questAbandonBinding;

    EventBinding<QuestTurnedInEvent> questTurnedInBinding;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(FireQuestPreviewEvent);
        questCompletedBinding = new EventBinding<QuestCompletedEvent>(HandleQuestCompleted);
        EventBus<QuestCompletedEvent>.Register(questCompletedBinding);

        questAbandonBinding = new EventBinding<QuestAbandonEvent>(HandleQuestAbandoned);
        EventBus<QuestAbandonEvent>.Register(questAbandonBinding);

        questTurnedInBinding = new EventBinding<QuestTurnedInEvent>(HandleQuestTurnedIn);
        EventBus<QuestTurnedInEvent>.Register(questTurnedInBinding);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(FireQuestPreviewEvent);
        EventBus<QuestCompletedEvent>.Deregister(questCompletedBinding);
        EventBus<QuestAbandonEvent>.Deregister(questAbandonBinding);
        EventBus<QuestTurnedInEvent>.Deregister(questTurnedInBinding);
    }

    private void HandleQuestCompleted(QuestCompletedEvent e)
    {
        if (e.questLogic.quest.shortObjective == text.text)
        {
            text.text = $"<s>{text.text}</s>";
        }
    }

    private void HandleQuestAbandoned(QuestAbandonEvent e)
    {
        if (e.questLogic == questLogic)
        {
            Destroy(gameObject);
        }
    }

    private void HandleQuestTurnedIn(QuestTurnedInEvent e)
    {
        if (e.questLogic == questLogic)
        {
            Destroy(gameObject);
        }
    }

    private void FireQuestPreviewEvent()
    {
        EventBus<QuestInProgressPreviewEvent>.Raise(new QuestInProgressPreviewEvent() { questLogic = this.questLogic }) ;
    }
}
