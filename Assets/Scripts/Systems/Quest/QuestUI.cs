using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject questPanel;
    [SerializeField] private TextMeshProUGUI questTitle;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private TextMeshProUGUI questObjective;

    private Quest currentQuest;


    EventBinding<QuestPreviewEvent> QuestPreviewEventBinding;

    private void OnEnable()
    {
        QuestPreviewEventBinding = new EventBinding<QuestPreviewEvent>(HandleQuestPreview);
        EventBus<QuestPreviewEvent>.Register(QuestPreviewEventBinding);
    }
    private void OnDisable()
    {
        EventBus<QuestPreviewEvent>.Deregister(QuestPreviewEventBinding);
    }

    private void HandleQuestPreview(QuestPreviewEvent e)
    {
        questPanel.SetActive(true);
        questTitle.text = e.quest.questName;
        questDescription.text = e.quest.description;
        questObjective.text = e.quest.objective;
        currentQuest = e.quest;
    }

    public void AcceptQuestEventPublish()
    {
        EventBus<QuestAcceptedEvent>.Raise(new QuestAcceptedEvent() { quest = currentQuest});
    }
}
