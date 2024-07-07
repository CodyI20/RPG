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
    EventBinding<NPCInteractInRangeEvent> NPCInteractInRangeEventBinding;


    private void OnEnable()
    {
        QuestInProgressPreviewEventBinding = new EventBinding<QuestInProgressPreviewEvent>(HandleQuestInProgressPreview);
        EventBus<QuestInProgressPreviewEvent>.Register(QuestInProgressPreviewEventBinding);
        NPCInteractInRangeEventBinding = new EventBinding<NPCInteractInRangeEvent>(HandleInteractInRange);
        EventBus<NPCInteractInRangeEvent>.Register(NPCInteractInRangeEventBinding);
    }
    private void OnDisable()
    {
        EventBus<QuestInProgressPreviewEvent>.Deregister(QuestInProgressPreviewEventBinding);
        EventBus<NPCInteractInRangeEvent>.Deregister(NPCInteractInRangeEventBinding);
    }

    private void HandleQuestInProgressPreview(QuestInProgressPreviewEvent e)
    {
        questInProgressPanel.SetActive(true);
        questInProgressTitle.text = e.questLogic.quest.questName;
        questInProgressDescription.text = e.questLogic.quest.description;
        questInProgressObjective.text = e.questLogic.quest.objective;
        currentlyHandledQuest = e.questLogic;
    }

    private void HandleInteractInRange(NPCInteractInRangeEvent e)
    {
        Deselect();
    }

    // Called by the UI button
    public void AbandonQuestEventPublish()
    {
        EventBus<QuestAbandonEvent>.Raise(new QuestAbandonEvent() { questLogic = currentlyHandledQuest });
    }

    private void Update()
    {
        TryDeselect();
    }

    // POLISH

    private void TryDeselect()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Deselect();
    }

    public void Deselect()
    {
        if(questInProgressPanel.activeSelf == false) return;
        EventBus<QuestPreviewExitEvent>.Raise(new QuestPreviewExitEvent());
        questInProgressPanel.SetActive(false);
    }
}
