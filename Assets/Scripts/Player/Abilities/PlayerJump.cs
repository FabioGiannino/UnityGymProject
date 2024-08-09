using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : PlayerAbilityBase
{
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private int maxConsecutiveJumps;
    [SerializeField]
    private float delayBetweenJump;

    private int currentJump;
    private bool stopForDelay;
    private Coroutine delayCoroutine;

    #region Mono
    private void OnEnable()
    {
        playerController.OnGroundLanded += OnGroundLanded;
        InputManager.Player.Jump.performed += OnInputPerformed;
        stopForDelay = false;
    }

    private void OnDisable()
    {
        playerController.OnGroundLanded -= OnGroundLanded;
        InputManager.Player.Jump.performed -= OnInputPerformed;
    }

    
    #endregion


    #region Overrides
    public override void OnInputDisable()
    {
        isPrevented = false;
    }

    public override void OnInputEnable()
    {
        isPrevented = true;
    }

    public override void StopAbility()
    {

    }
    #endregion

    #region Callbacks
    private void OnInputPerformed(InputAction.CallbackContext context)
    {
        if (currentJump == maxConsecutiveJumps) return;
        if (isPrevented) return;
        Jump();
    }
    private void OnGroundLanded()
    {
        currentJump = 0;
    }
    #endregion

    #region PrivateMethods
    private void Jump()
    {
        if (stopForDelay) return;
        Vector2 velocity = playerController.PlayerRigidBody.velocity;
        velocity.y = Math.Abs(jumpSpeed);
        playerController.PlayerRigidBody.velocity = velocity;
        currentJump += 1;
        delayCoroutine = StartCoroutine(JumpDelay());
    }

    private IEnumerator JumpDelay()
    {
        stopForDelay = true;
        yield return new WaitForSeconds(delayBetweenJump);
        stopForDelay = false;
    }
    #endregion
}
