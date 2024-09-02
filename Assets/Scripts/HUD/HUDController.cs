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
        GlobalEventSystem.AddListener(EventName.PlayerUpdateLevelState, UpdateStateListener);
    }
    private void OnDisable()
    {
        GlobalEventSystem.RemoveListener(EventName.PlayerHealthUpdate, HealthUpdateListener);
        GlobalEventSystem.RemoveListener(EventName.PlayerUpdateLevelState, UpdateStateListener);
    }

    #region HealthUI
    private void HealthUpdateListener(GlobalEventArgs message)
    {
        GlobalEventArgsFactory.PlayerHealthUpdateParser(message, out float maxHP, out float currentHP);
        if (!(healthUI.MaxHealth == maxHP))
        {
            healthUI.MaxHealth = maxHP;
        }
        currentHP = Mathf.Clamp(currentHP, 0.0f, maxHP);
        healthUI.CurrentHealth = currentHP;
    }
    #endregion

    #region StateUI
    private void UpdateStateListener(GlobalEventArgs message)
    {
        GlobalEventArgsFactory.PlayerUpdateLevelStateParser(message, out StateEffect stateName, out float stateLevelInPercentage);
        VisualElement icon = GetIcon(stateName);
        stateLevelInPercentage = Mathf.Clamp(stateLevelInPercentage, 0.0f, 1.0f);
        icon.style.height = Length.Percent(stateLevelInPercentage*100);
        

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
