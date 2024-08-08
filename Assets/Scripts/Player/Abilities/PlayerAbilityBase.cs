using UnityEngine;

public abstract class PlayerAbilityBase : MonoBehaviour
{

    protected PlayerController playerController;

    protected bool isPrevented;

    public abstract void OnInputEnable();
    public abstract void OnInputDisable();
    public abstract void StopAbility();

    public virtual void Init(PlayerController controller)
    {
        playerController = controller;
    }
}
