using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    [Header("Quest In Progress")]
    [SerializeField] private GameObject questInProgressPanel;
    [SerializeField] private TextMeshProUGUI questInProgressTitle;
    [SerializeField] private TextMeshProUGUI questInProgressDescription;
    [SerializeField] private TextMeshProUGUI questInProgressObjective;
    private QuestLogic currentlyHandledQuest;


    EventBinding<QuestInProgressPreviewEvent> QuestInProgressPreviewEventBinding;


    private void OnEnable()
    {
        QuestInProgressPreviewEventBinding = new EventBinding<QuestInProgressPreviewEvent>(HandleQuestInProgressPreview);
        EventBus<QuestInProgressPreviewEvent>.Register(QuestInProgressPreviewEventBinding);
    }
    private void OnDisable()
    {
        EventBus<QuestInProgressPreviewEvent>.Deregister(QuestInProgressPreviewEventBinding);
    }

    private void HandleQuestInProgressPreview(QuestInProgressPreviewEvent e)
    {
        questInProgressPanel.SetActive(true);
        questInProgressTitle.text = e.questLogic.quest.questName;
        questInProgressDescription.text = e.questLogic.quest.description;
        questInProgressObjective.text = e.questLogic.quest.objective;
        currentlyHandledQuest = e.questLogic;
    }

    // Called by the UI button
    public void AbandonQuestEventPublish()
    {
        EventBus<QuestAbandonEvent>.Raise(new QuestAbandonEvent() { questLogic = currentlyHandledQuest });
    }
}
