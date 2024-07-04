using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public Image radialImage;
    public Image abilityIcon;
    [HideInInspector] public int index;
    [SerializeField] private Key keyToPress;
    [SerializeField] private TextMeshProUGUI hotkeyDisplay;
    public Key key
    {
        get => keyToPress;
        private set => keyToPress = value;
    }

    public event Action<int> OnButtonPressed = delegate { };

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => OnButtonPressed(index));
    }

    void Update()
    {
        if (Keyboard.current[key].wasPressedThisFrame)
        {
            OnButtonPressed(index);
        }
    }

    public void RegisterListener(Action<int> listener)
    {
        OnButtonPressed += listener;
    }

    public void Initialize(int index, Key key)
    {
        this.key = key;
        this.index = index;
        if (key.ToString().Contains("Digit"))
        {
            hotkeyDisplay.text = key.ToString().Replace("Digit", "");
        }else if (key.ToString().Contains("Alpha"))
        {
            hotkeyDisplay.text = key.ToString().Replace("Alpha", "");
        }
        else
            hotkeyDisplay.text = key.ToString();
    }

    public void UpdateButtonSprite(Sprite newIcon)
    {
        abilityIcon.sprite = newIcon;
    }

    public void UpdateRadialFill(float progress)
    {
        if (radialImage)
        {
            radialImage.fillAmount = progress;
        }
    }
}
