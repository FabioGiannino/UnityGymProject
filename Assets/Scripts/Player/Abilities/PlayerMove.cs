using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : PlayerAbilityBase
{
    #region SerializeFields
    [SerializeField]
    private float maxSpeed;
    #endregion

    #region Private Attributes
    private InputAction move;
    private bool wasWalking;
    private const float turnSpeedOffset = 0.05f;
    #endregion

    #region Mono
    private void OnEnable()
    {
        move = InputManager.Player.Move;
        wasWalking = false;
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
    }
    #endregion

    #region Private Methods
    private void Move()
    {
        float speed = maxSpeed * move.ReadValue<float>();
        SetSpeed(speed);
        playerController.IsWalking = speed != 0;        
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
}
