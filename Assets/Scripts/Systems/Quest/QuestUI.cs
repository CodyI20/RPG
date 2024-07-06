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

    private QuestLogic currentQuest;


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
        questTitle.text = e.questLogic.quest.questName;
        questDescription.text = e.questLogic.quest.description;
        questObjective.text = e.questLogic.quest.objective;
        currentQuest = e.questLogic;
    }

    public void AcceptQuestEventPublish()
    {
        EventBus<QuestAcceptedEvent>.Raise(new QuestAcceptedEvent() { questLogic = currentQuest});
    }
}
