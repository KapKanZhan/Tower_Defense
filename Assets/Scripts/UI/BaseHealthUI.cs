using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class BaseHealthUI : MonoBehaviour
{
    [SerializeField] private BaseHealth baseHealth;
    [SerializeField] private TMP_Text healthText;

    private void Start()
    {
        if (baseHealth != null)
        {
            baseHealth.OnHealthChanged += UpdateHealthText;
            UpdateHealthText(baseHealth.CurrentHealth, baseHealth.MaxHealth);
        }
    }

    private void OnDestroy()
    {
        if (baseHealth != null)
        {
            baseHealth.OnHealthChanged -= UpdateHealthText;
        }
    }

    private void UpdateHealthText(int current, int max)
    {
        healthText.text = $"{current}/{max}";
    }
}
