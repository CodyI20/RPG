using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestOverviewDisplay : MonoBehaviour
{
    [SerializeField] private GameObject questOverviewTextButtonPrefab;
    [SerializeField] private Transform questOverviewPanel;

    EventBinding<QuestAcceptedEvent> QuestAcceptedEventBinding;

    private void OnEnable()
    {
        QuestAcceptedEventBinding = new EventBinding<QuestAcceptedEvent>(HandleQuestAccepted);
        EventBus<QuestAcceptedEvent>.Register(QuestAcceptedEventBinding);
    }
    private void OnDisable()
    {
        EventBus<QuestAcceptedEvent>.Deregister(QuestAcceptedEventBinding);
    }

    private void HandleQuestAccepted(QuestAcceptedEvent e)
    {
        GameObject questOverviewTextButton = Instantiate(questOverviewTextButtonPrefab, questOverviewPanel);
        questOverviewTextButton.GetComponent<TextMeshProUGUI>().text = e.questLogic.quest.shortObjective;
        questOverviewTextButton.GetComponent<QuestOverviewText>().questLogic = e.questLogic;
    }
}
