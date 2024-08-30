using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    private HealthUI healthUI;

    private VisualElement iceIcon;

    private void Awake()
    {
        healthUI = GetComponent<UIDocument>().rootVisualElement.Q<HealthUI>("HealthUI");
        iceIcon = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("IceIcon");
    }

    private void OnEnable()
    {
        GlobalEventSystem.AddListener(EventName.PlayerHealthUpdate, HealthUpdateListener);
        GlobalEventSystem.AddListener(EventName.PlayerUpdateState, UpdateStateListener);
    }
    private void OnDisable()
    {
        GlobalEventSystem.RemoveListener(EventName.PlayerHealthUpdate, HealthUpdateListener);
        GlobalEventSystem.RemoveListener(EventName.PlayerUpdateState, UpdateStateListener);
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
    private void UpdateStateListener(EventArgs message)
    {
        EventArgsFactory.PlayerUpdateStateParser(message, out StateEffect stateName, out bool isAffected);
        if (isAffected)
        {
            iceIcon.visible = true;
        }
        else
        {
            iceIcon.visible = false;
        }
    }
}
