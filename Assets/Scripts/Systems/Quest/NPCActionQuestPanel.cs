using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCActionQuestPanel : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectParentToEnable;
    [SerializeField] private GameObject questTextPrefab;
    [SerializeField] private GameObject QuestPanel;
    [SerializeField] private GameObject QuestPanelNoQuests;

    [Space(10)]
    [SerializeField] private GameObject NPCInProgressQuestsPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private Button completeQuestButton;

    [Space(10)]
    [SerializeField] private GameObject NPCAvailableQuestPanel;
    [SerializeField] private TextMeshProUGUI availableQuestTitle;
    [SerializeField] private TextMeshProUGUI availableQuestDescription;
    [SerializeField] private TextMeshProUGUI availableQuestObjective;

    private List<QuestLogic> questsAlreadyAdded = new List<QuestLogic>();
    private List<QuestLogic> questsAccepted = new List<QuestLogic>();
    private QuestLogic currentlyHandledQuest;

    EventBinding<NPCInteractInRangeEvent> InteractInRangeEvent;
    EventBinding<QuestPreviewEvent> PreviewEvent;
    EventBinding<QuestAbandonEvent> AbandonEvent;
    EventBinding<QuestCompletedEvent> CompletedEvent;

    private void OnEnable()
    {
        InteractInRangeEvent = new EventBinding<NPCInteractInRangeEvent>(HandleInteractInRange);
        EventBus<NPCInteractInRangeEvent>.Register(InteractInRangeEvent);
        PreviewEvent = new EventBinding<QuestPreviewEvent>(HandleQuestPreview);
        EventBus<QuestPreviewEvent>.Register(PreviewEvent);
        AbandonEvent = new EventBinding<QuestAbandonEvent>(HandleQuestAbandon);
        EventBus<QuestAbandonEvent>.Register(AbandonEvent);
        CompletedEvent = new EventBinding<QuestCompletedEvent>(HandleQuestComplete);
        EventBus<QuestCompletedEvent>.Register(CompletedEvent);
    }
    private void OnDisable()
    {
        EventBus<NPCInteractInRangeEvent>.Deregister(InteractInRangeEvent);
        EventBus<QuestPreviewEvent>.Deregister(PreviewEvent);
        EventBus<QuestAbandonEvent>.Deregister(AbandonEvent);
        EventBus<QuestCompletedEvent>.Deregister(CompletedEvent);
    }

    private void HandleInteractInRange(NPCInteractInRangeEvent e)
    {
        if (e.questGiver == null) return;
        if (e.selection == e.questGiver.transform)
        {
            gameObjectParentToEnable.SetActive(true);
            NPCAvailableQuestPanel.SetActive(false);
            NPCInProgressQuestsPanel.SetActive(false);
            if (e.questsCount == 0)
            {
                QuestPanel.SetActive(false);
                QuestPanelNoQuests.SetActive(true);
                return;
            }
            else
                QuestPanelNoQuests.SetActive(false);
            QuestPanel.SetActive(true);
            foreach (QuestLogic quest in e.questGiver.questLogics)
            {
                if (questsAlreadyAdded.Contains(quest)) continue;
                questsAlreadyAdded.Add(quest);
                GameObject questText = Instantiate(questTextPrefab, QuestPanel.transform);
                questText.GetComponent<TextMeshProUGUI>().SetText(quest.quest.questName);
                questText.GetComponent<QuestButton>().questLogic = quest;
            }
        }
    }

    private void HandleQuestPreview(QuestPreviewEvent e)
    {
        gameObjectParentToEnable.SetActive(true);
        QuestPanel.SetActive(false);
        QuestPanelNoQuests.SetActive(false);
        if (questsAccepted.Contains(e.questLogic))
        {
            NPCAvailableQuestPanel.SetActive(false);
            NPCInProgressQuestsPanel.SetActive(true);
            titleText.text = e.questLogic.quest.questName;
            descriptionText.text = e.questLogic.quest.description;
            objectiveText.text = e.questLogic.quest.objective;
        }
        else
        {
            NPCInProgressQuestsPanel.SetActive(false);
            NPCAvailableQuestPanel.SetActive(true);
            availableQuestTitle.text = e.questLogic.quest.questName;
            availableQuestDescription.text = e.questLogic.quest.description;
            availableQuestObjective.text = e.questLogic.quest.objective;
        }
        currentlyHandledQuest = e.questLogic;
    }

    private void HandleQuestAbandon(QuestAbandonEvent e)
    {
        if (questsAccepted.Contains(e.questLogic))
        {
            completeQuestButton.interactable = false;
            questsAccepted.Remove(e.questLogic);
            gameObjectParentToEnable.SetActive(false);
        }
    }

    private void HandleQuestComplete(QuestCompletedEvent e)
    {
        if (questsAccepted.Contains(e.questLogic))
        {
            completeQuestButton.interactable = true;
        }
    }

    //Called by the UI Button
    public void AcceptQuestEventPublish()
    {
        questsAccepted.Add(currentlyHandledQuest);
        EventBus<QuestAcceptedEvent>.Raise(new QuestAcceptedEvent() { questLogic = currentlyHandledQuest });
    }

    public void CompleteQuestActions()
    {
        questsAccepted.Remove(currentlyHandledQuest);
        gameObjectParentToEnable.SetActive(false);
    }
}
