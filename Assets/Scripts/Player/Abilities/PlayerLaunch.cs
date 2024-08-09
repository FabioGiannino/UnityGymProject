using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLaunch : PlayerAbilityBase
{

    [SerializeField]
    private Camera mainCamera;

    private Vector2 mousePos;

    private void OnEnable()
    {
        if (mainCamera == null)
            mainCamera = (Camera)FindObjectOfType(typeof(Camera));

        InputManager.Player.Launch.performed += OnInputPerformed;

    }

    private void OnDisable()
    {
        InputManager.Player.Launch.performed -= OnInputPerformed;
    }

    private void OnInputPerformed(InputAction.CallbackContext context)
    {
        if (isPrevented) return;
        mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Debug.Log("Press LeftMouse " + mousePos);
        if (InputManager.Player.Launch.IsPressed())
        {
            Debug.Log("IsPressed LeftMouse");
        }
    }


    #region Override
    public override void OnInputDisable()
    {
        isPrevented = true;
    }

    public override void OnInputEnable()
    {
        isPrevented = false;
    }

    public override void StopAbility()
    {
        
    }
    #endregion

}
