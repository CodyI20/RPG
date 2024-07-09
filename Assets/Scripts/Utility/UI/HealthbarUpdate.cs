using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUpdate : MonoBehaviour
{
    [SerializeField] private Image fill;

    protected void UpdateHealthBar(float currentHealthPercentage)
    {
        fill.fillAmount = currentHealthPercentage;
    }
}
