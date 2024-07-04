using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    public Image radialImage;
    public Image abilityIcon;
    [HideInInspector] public int index;
    [SerializeField] private Key keyToPress;
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
