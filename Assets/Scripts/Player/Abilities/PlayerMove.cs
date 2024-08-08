using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;

public class PlayerMove : PlayerAbilityBase
{
    [SerializeField]
    private float maxSpeed;

    private InputAction move;


    #region Mono
    private void OnEnable()
    {
        move = InputManager.Player.Move;
        
    }
    private void Update()
    {
        if(isPrevented) return;
        Move();
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

    private void Move()
    {
        float speed = maxSpeed * move.ReadValue<float>();
        SetSpeed(speed);
    }

    private void SetSpeed(float speed) 
    { 
        Vector2 velocity = new Vector2(speed, playerController.PlayerRigidBody.velocity.y);
        playerController.PlayerRigidBody.velocity = velocity;
    }

}
