using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuestButton : MonoBehaviour
{
    [HideInInspector] public QuestLogic questLogic;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        EventBus<QuestPreviewEvent>.Raise(new QuestPreviewEvent { questLogic = this.questLogic });
    }

}
