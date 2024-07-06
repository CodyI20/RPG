using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuestButton : MonoBehaviour
{
    [HideInInspector] public QuestLogic questLogic;
    private Button button;

    EventBinding<QuestAbandonEvent> abandonEvent;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        abandonEvent = new EventBinding<QuestAbandonEvent>(HandleQuestAbandon);
        EventBus<QuestAbandonEvent>.Register(abandonEvent);
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        EventBus<QuestAbandonEvent>.Deregister(abandonEvent);
        button.onClick.RemoveListener(OnButtonClicked);
    }

    private void HandleQuestAbandon(QuestAbandonEvent e)
    {
        if (e.questLogic == questLogic)
        {
            Destroy(gameObject);
        }
    }

    private void OnButtonClicked()
    {
        EventBus<QuestPreviewEvent>.Raise(new QuestPreviewEvent { questLogic = this.questLogic });
    }

}
