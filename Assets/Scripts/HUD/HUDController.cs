using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    private HealthUI healthUI;

    private VisualElement iceIcon;
    private VisualElement fireIcon;
    private VisualElement poisonIcon;

    private void Awake()
    {
        healthUI = GetComponent<UIDocument>().rootVisualElement.Q<HealthUI>("HealthUI");
        iceIcon = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("IceIcon");
        fireIcon = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("FireIcon");
        poisonIcon = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("PoisonIcon");
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

    #region HealthUI
    private void HealthUpdateListener(EventArgs message)
    {
        EventArgsFactory.PlayerHealthUpdateParser(message, out float maxHP, out float currentHP);
        if (!(healthUI.MaxHealth == maxHP))
        {
            healthUI.MaxHealth = maxHP;
        }
        healthUI.CurrentHealth = currentHP;
    }
    #endregion

    #region StateUI
    private void UpdateStateListener(EventArgs message)
    {
        EventArgsFactory.PlayerUpdateStateParser(message, out StateEffect stateName, out bool isAffected);
        VisualElement icon = GetIcon(stateName);
        if(isAffected)
        {
            icon.visible = true;
        }
        else
        {
            icon.visible = false;
        }
    }

    private VisualElement GetIcon(StateEffect stateEffect)
    {
        switch (stateEffect)
        {
            case StateEffect.Cold: return iceIcon;
            case StateEffect.Poison: return poisonIcon;
            case StateEffect.Fire: return fireIcon;
            default: return null;
        }
    }
    #endregion
}
