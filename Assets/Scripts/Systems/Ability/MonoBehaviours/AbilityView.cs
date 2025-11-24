using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityView : MonoBehaviour
{
    [SerializeField] public AbilityButton[] buttons;

    readonly Key[] keys = { Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4, Key.Digit5 };

    void Awake()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i >= keys.Length)
            {
                Debug.LogError("Not enough keycodes for the number of buttons.");
                break;
            }
            if (buttons[i].key == Key.None)
                buttons[i].Initialize(i, keys[i]);
        }
    }

    public void UpdateRadial(IList<Ability> abilities)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < abilities.Count)
            {
                var progress = abilities[i].CooldownTimer.Progress;
                if (float.IsNaN(progress))
                {
                    progress = 0;
                }
                buttons[i].UpdateRadialFill(progress);
            }
            else
            {
                buttons[i].UpdateRadialFill(0);
            }
        }
    }

    public void UpdateButtonDetails(IList<Ability> abilities)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < abilities.Count)
            {
                buttons[i].UpdateButtonSprite(abilities[i].data.icon);
                buttons[i].UpdateTooltip(abilities[i].data.fullName, abilities[i].data.tooltipDescription);
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }
}
