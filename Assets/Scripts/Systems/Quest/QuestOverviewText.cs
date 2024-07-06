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
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(FireQuestPreviewEvent);
        EventBus<QuestCompletedEvent>.Deregister(questCompletedBinding);
    }

    private void HandleQuestCompleted(QuestCompletedEvent e)
    {
        if (e.questLogic.quest.shortObjective == text.text)
        {
            text.text = $"<s>{text.text}</s>";
        }
    }

    private void FireQuestPreviewEvent()
    {
        EventBus<QuestInProgressPreviewEvent>.Raise(new QuestInProgressPreviewEvent() { questLogic = this.questLogic }) ;
    }
}
