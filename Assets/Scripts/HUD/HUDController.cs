using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    private HealthUI healthUI;
    private void Awake()
    {
        healthUI = GetComponent<UIDocument>().rootVisualElement.Q<HealthUI>("HealthUI");
    }

    private void OnEnable()
    {
        GlobalEventSystem.AddListener(EventName.PlayerHealthUpdate, HealthUpdateListener);
    }
    private void OnDisable()
    {
        GlobalEventSystem.RemoveListener(EventName.PlayerHealthUpdate, HealthUpdateListener);
    }


    private void HealthUpdateListener(EventArgs message)
    {
        EventArgsFactory.PlayerHealthUpdateParser(message, out float maxHP, out float currentHP);
        if (!(healthUI.MaxHealth == maxHP))
        {
            healthUI.MaxHealth = maxHP;
        }
        healthUI.CurrentHealth = currentHP;
    }
}
