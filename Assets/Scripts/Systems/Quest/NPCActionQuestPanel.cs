using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCActionQuestPanel : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectParentToEnable;
    [SerializeField] private GameObject QuestPanel;
    [SerializeField] private GameObject questTextPrefab;

    private List<QuestLogic> questsAlreadyAdded = new List<QuestLogic>();

    EventBinding<NPCInteractInRangeEvent> InteractInRangeEvent;
    EventBinding<QuestPreviewEvent> PreviewEvent;

    private void OnEnable()
    {
        InteractInRangeEvent = new EventBinding<NPCInteractInRangeEvent>(HandleInteractInRange);
        EventBus<NPCInteractInRangeEvent>.Register(InteractInRangeEvent);
        PreviewEvent = new EventBinding<QuestPreviewEvent>(e => gameObjectParentToEnable.SetActive(false));
        EventBus<QuestPreviewEvent>.Register(PreviewEvent);
    }
    private void OnDisable()
    {
        EventBus<NPCInteractInRangeEvent>.Deregister(InteractInRangeEvent);
        EventBus<QuestPreviewEvent>.Deregister(PreviewEvent);
    }

    private void HandleInteractInRange(NPCInteractInRangeEvent e)
    {
        if (e.questGiver == null) return;
        if (e.selection == e.questGiver.transform)
        {
            gameObjectParentToEnable.SetActive(true);
            foreach (QuestLogic quest in e.questGiver.questLogics)
            {
                if(questsAlreadyAdded.Contains(quest)) continue;
                questsAlreadyAdded.Add(quest);
                GameObject questText = Instantiate(questTextPrefab, QuestPanel.transform);
                questText.GetComponent<TextMeshProUGUI>().SetText(quest.quest.questName);
                questText.GetComponent<QuestButton>().questLogic = quest;
            }
        }
    }
}
