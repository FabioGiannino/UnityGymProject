using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLaunch : PlayerAbilityBase
{

    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private float maxLaunchForce;
    [SerializeField]
    private float minLaunchForce; 
    [SerializeField]
    protected AnimationCurve incrementationForceCurve;
    [SerializeField]
    private float maxEvaluationInputTime;
    [SerializeField]
    private int maxConsecutiveLaunches;


    private int currentConsecutiveLaunch;
    private Vector2 forceVector;
    private Coroutine launchCoroutine;

    #region Mono
    private void OnEnable()
    {
        if (mainCamera == null)
            mainCamera = (Camera)FindObjectOfType(typeof(Camera));
        currentConsecutiveLaunch = 0;

        InputManager.Player.Launch.performed += OnInputPerformed;
        playerController.OnGroundLanded += OnGroundLanded;
    }

    private void OnDisable()
    {
        InputManager.Player.Launch.performed -= OnInputPerformed;
        playerController.OnGroundLanded -= OnGroundLanded;
    }
    #endregion

    #region Input
    private void OnInputPerformed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!CanBeLaunched()) return;
        Time.timeScale = 0;
        playerController.PlayerRigidBody.velocity = Vector2.zero;
        GlobalEventSystem.CastEvent(EventName.LaunchPlayerStart, EventArgsFactory.LaunchPlayerStartFactory(maxEvaluationInputTime));
        launchCoroutine = StartCoroutine(LaunchCoroutine());
    }
    #endregion

    #region PrivateMethods
    private bool CanBeLaunched()
    {
        return !isPrevented && currentConsecutiveLaunch < maxConsecutiveLaunches;
    }


    private void SetForceNormalized()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        forceVector = mousePos - (Vector2)playerController.transform.position;
        forceVector = forceVector.normalized;
    }

    private void Launch()
    {
        Time.timeScale = 1;
        playerController.PlayerRigidBody.AddForce(forceVector, ForceMode2D.Impulse);
        playerController.OnLaunchStarted?.Invoke();
        GlobalEventSystem.CastEvent(EventName.LaunchPlayerStop, EventArgsFactory.LaunchPlayerStopFactory());
    }
    #endregion

    #region Coroutine
    private IEnumerator LaunchCoroutine()
    {
        currentConsecutiveLaunch++;
        if (!InputManager.Player.Launch.IsPressed())
        {
            SetForceNormalized();
            forceVector *= minLaunchForce;            
            Launch();
            StopAbility();
            yield break;
        }

        float inputTime = 0;
        float evaluate = 0;
        while (inputTime< maxEvaluationInputTime && InputManager.Player.Launch.IsPressed())
        {            
            inputTime += Time.unscaledDeltaTime;
            evaluate = Mathf.Lerp(minLaunchForce, maxLaunchForce, incrementationForceCurve.Evaluate(inputTime / maxEvaluationInputTime));
            yield return null;
        }
        SetForceNormalized();
        forceVector *= evaluate;
        Launch();
        StopAbility();
        
    }
    #endregion

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
        if(launchCoroutine != null)
        {
            StopCoroutine(launchCoroutine);
        }
    }
    #endregion

    #region Callbacks

    private void OnGroundLanded()
    {
        currentConsecutiveLaunch = 0;
    }
    #endregion

}
