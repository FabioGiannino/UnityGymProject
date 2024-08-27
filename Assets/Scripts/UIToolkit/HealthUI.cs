using UnityEngine.UIElements;
using UnityEngine;

public class HealthUI : VisualElement
{
    public new class UXmlFactory : UxmlFactory<HealthUI, UxmlTraits>
    {
    }
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlFloatAttributeDescription maxHealth = new UxmlFloatAttributeDescription() 
        {
            name = "Max_Health"
        };
        UxmlFloatAttributeDescription currentHealth = new UxmlFloatAttributeDescription() 
        {
            name = "Current_Health"
        };
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)  
        {
            base.Init(ve, bag, cc);

            (ve as HealthUI).CurrentHealth = currentHealth.GetValueFromBag(bag, cc);
            (ve as HealthUI).MaxHealth = maxHealth.GetValueFromBag(bag, cc);
        }
    }
    private VisualTreeAsset healthBarTemplate;
    private VisualElement healthBarContainer;

    private float currentHealth=0;
    private float maxHealth=100;

    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public float CurrentHealth { get { return currentHealth; } set { currentHealth = value; SetHealthUI(); } }

    public HealthUI()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("HealthBarStyleSheet"));
        healthBarTemplate = Resources.Load<VisualTreeAsset>("HealthBar");
        healthBarContainer = new VisualElement();
        healthBarContainer.name = "Health";
        healthBarContainer.AddToClassList("healthBar");
        hierarchy.Add(healthBarContainer);
    }

    private void SetHealthUI()
    {
        healthBarContainer.Clear();
        var hb = healthBarTemplate.Instantiate();
        hb.name = "HealthBarTemplate";
        hb.AddToClassList("healthBar");
        SetProgressBarParam(hb.Q<VisualElement>("CurrentHealthBar"), currentHealth);
        healthBarContainer.Add(hb);
    }

    private void SetProgressBarParam(VisualElement progressBar, float value)
    {
        if (maxHealth == 0) return;
        float percentValue = value / maxHealth * 100;
        progressBar.style.width = Length.Percent(percentValue);
    }

}
