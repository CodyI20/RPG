using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class QuestListDropdownButton : MonoBehaviour
{
    Button button;
    Image image;

    [SerializeField] private Sprite shownMenuSprite;
    [SerializeField] private Sprite hiddenMenuSprite;

    [SerializeField] private GameObject menu;

    private bool isMenuShown = true;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        image = GetComponent<Image>();
        image.sprite = shownMenuSprite;

        isMenuShown = true;
        menu.SetActive(isMenuShown);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        isMenuShown = !isMenuShown;
        image.sprite = isMenuShown ? shownMenuSprite : hiddenMenuSprite;
        menu.SetActive(isMenuShown);
    }
}
