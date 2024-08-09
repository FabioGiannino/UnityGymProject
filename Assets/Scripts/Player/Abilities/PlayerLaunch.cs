using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLaunch : PlayerAbilityBase
{

    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private float launchForce;

    private Vector2 mousePos;

    private void OnEnable()
    {
        if (mainCamera == null)
            mainCamera = (Camera)FindObjectOfType(typeof(Camera));

        InputManager.Player.Launch.performed += OnInputPerformed;
        playerController.OnGroundLanded += OnGroundLanded;
    }

    private void OnDisable()
    {
        InputManager.Player.Launch.performed -= OnInputPerformed;
        playerController.OnGroundLanded -= OnGroundLanded;
    }

    private void OnInputPerformed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!CanBeLaunched()) return;
        Launch();
    }

    private bool CanBeLaunched()
    {
        return !isPrevented;
    }

    private void Launch()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 forceVector = mousePos - (Vector2)playerController.transform.position;
        forceVector = forceVector.normalized;
        forceVector *= launchForce;
        playerController.PlayerRigidBody.AddForce(forceVector,ForceMode2D.Impulse);
        playerController.OnLaunchStarted?.Invoke();
        /*
        mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (InputManager.Player.Launch.IsPressed())
        {
            Debug.Log("IsPressed LeftMouse");
        }
        */
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

    #region Callbacks
    private void OnGroundLanded()
    {
        playerController.OnLaunchEnded?.Invoke();
    }
    #endregion

}
