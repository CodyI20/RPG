using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityTooltipShow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject tooltipPanel;

    public TextMeshProUGUI TitleText
    {
        get => titleText;
        set => titleText = value;
    }

    public TextMeshProUGUI DescriptionText
    {
        get => descriptionText;
        set => descriptionText = value;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipPanel.SetActive(false);
    }
}
