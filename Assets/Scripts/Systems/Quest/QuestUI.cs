using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    [Header("Quest Preview")]
    [SerializeField] private GameObject questPreviewPanel;
    [SerializeField] private TextMeshProUGUI questTitle;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private TextMeshProUGUI questObjective;

    [Space(10)]
    [Header("Quest In Progress")]
    [SerializeField] private GameObject questInProgressPanel;
    [SerializeField] private TextMeshProUGUI questInProgressTitle;
    [SerializeField] private TextMeshProUGUI questInProgressDescription;
    [SerializeField] private TextMeshProUGUI questInProgressObjective;
    private QuestLogic currentQuest;


    EventBinding<QuestPreviewEvent> QuestPreviewEventBinding;
    EventBinding<QuestInProgressPreviewEvent> QuestInProgressPreviewEventBinding;

    private void OnEnable()
    {
        QuestPreviewEventBinding = new EventBinding<QuestPreviewEvent>(HandleQuestPreview);
        EventBus<QuestPreviewEvent>.Register(QuestPreviewEventBinding);

        QuestInProgressPreviewEventBinding = new EventBinding<QuestInProgressPreviewEvent>(HandleQuestInProgressPreview);
        EventBus<QuestInProgressPreviewEvent>.Register(QuestInProgressPreviewEventBinding);
    }
    private void OnDisable()
    {
        EventBus<QuestPreviewEvent>.Deregister(QuestPreviewEventBinding);
        EventBus<QuestInProgressPreviewEvent>.Deregister(QuestInProgressPreviewEventBinding);
    }

    private void HandleQuestPreview(QuestPreviewEvent e)
    {
        questInProgressPanel.SetActive(false);
        questPreviewPanel.SetActive(true);
        questTitle.text = e.questLogic.quest.questName;
        questDescription.text = e.questLogic.quest.description;
        questObjective.text = e.questLogic.quest.objective;
        currentQuest = e.questLogic;
    }

    private void HandleQuestInProgressPreview(QuestInProgressPreviewEvent e)
    {
        questPreviewPanel.SetActive(false);
        questInProgressPanel.SetActive(true);
        questInProgressTitle.text = e.questLogic.quest.questName;
        questInProgressDescription.text = e.questLogic.quest.description;
        questInProgressObjective.text = e.questLogic.quest.objective;
        currentQuest = e.questLogic;
    }

    //Called by the UI Button
    public void AcceptQuestEventPublish()
    {
        EventBus<QuestAcceptedEvent>.Raise(new QuestAcceptedEvent() { questLogic = currentQuest});
    }
}
