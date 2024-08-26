using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : PlayerAbilityBase
{
    #region SerializeFields
    [SerializeField]
    private float maxSpeed;
    #endregion

    #region Private Attributes
    private float currentMaxSpeed;
    private InputAction move;
    private bool wasWalking;
    private const float turnSpeedOffset = 0.05f;
    #endregion

    #region Mono
    private void OnEnable()
    {
        currentMaxSpeed = maxSpeed;
        move = InputManager.Player.Move;
        wasWalking = false;
        playerController.OnLaunchStarted += OnLaunchStarted;
        playerController.OnGroundLanded += OnGroundLanded;
        playerController.OnIceStateEntered += OnIceStateEntered;
        playerController.OnIceStateExited += OnIceStateExited;
    }
    private void OnDisable()
    {
        playerController.OnLaunchStarted -= OnLaunchStarted;
        playerController.OnGroundLanded -= OnGroundLanded;
    }

    private void Update()
    {
        if(isPrevented) return;

        Move();
        Turn();
        HandleEvent();
    }
    #endregion

    #region Override
    public override void OnInputDisable()
    {
        isPrevented = true;
        StopAbility();
    }

    public override void OnInputEnable()
    {
        isPrevented = false;
    }

    public override void StopAbility()
    {
        SetSpeed(0);
        playerController.OnWalkEnded?.Invoke();
    }
    #endregion

    #region Private Methods
    private void Move()
    {
        float speed = currentMaxSpeed * move.ReadValue<float>();
        playerController.IsWalking = speed != 0;
        SetSpeed(speed);        
    }
    private void Turn()
    {
        Vector2 velocity = playerController.PlayerRigidBody.velocity;

        Vector3 eulerRotation = playerController.PlayerTransform.localEulerAngles;
        if (velocity.x < -turnSpeedOffset)
        {
            eulerRotation.y = -180;
        }
        else if (velocity.x > turnSpeedOffset)
        {
            eulerRotation.y = 0;
        }
        playerController.PlayerTransform.localEulerAngles = eulerRotation;  //ruoto il player cambiando la sua transform.eulerangle
    }


    private void HandleEvent()
    {
        if (!wasWalking && playerController.IsWalking)
            playerController.OnWalkStarted?.Invoke();
        if (!playerController.IsWalking && wasWalking)
            playerController.OnWalkEnded?.Invoke();
        wasWalking = playerController.IsWalking;
    }

    private void SetSpeed(float speed) 
    { 
        Vector2 velocity = new Vector2(speed, playerController.PlayerRigidBody.velocity.y);
        playerController.PlayerRigidBody.velocity = velocity;
    }
    #endregion

    #region Callbacks
    private void OnLaunchStarted()
    {
        isPrevented = true;
    }
    private void OnGroundLanded()
    {
        isPrevented = false;
    }
    private void OnIceStateEntered()
    {
        currentMaxSpeed = currentMaxSpeed * 0.5f;
    }
    private void OnIceStateExited()
    {
        currentMaxSpeed = maxSpeed;
    }
    #endregion
}
