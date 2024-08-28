using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Private Members
    private PlayerAbilityBase[] abilities;
    #endregion

    #region Serialize Fields
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Rigidbody2D playerRigidBody;
    [SerializeField]
    private Collider2D playerPhysicsCollider;
    [SerializeField]
    private float timerInputDisableAfterHit;
    #endregion

    #region Properties
    public Transform PlayerTransform { get { return playerTransform; } }
    public Rigidbody2D PlayerRigidBody { get {  return playerRigidBody; } }
    public Collider2D PlayerPhysicsCollider { get { return playerPhysicsCollider; } }
    #endregion

    #region Mono
    private void Start()
    {
        abilities = GetComponentsInChildren<PlayerAbilityBase>();
        foreach (PlayerAbilityBase ability in abilities)
        {
            ability.Init(this);
            ability.enabled = true;
        }
        DebugAbilities();

        OnDamageTaken += InternalOnDamageTaken;
    }
    #endregion

    #region Ability: Walk
    public Action OnWalkStarted { get; set; }
    public Action OnWalkEnded { get; set;}
    public bool IsWalking { get; set; }
    public Action OnIceStateEntered;
    public Action OnIceStateExited;
    #endregion

    #region Ability: Jump
    public bool IsJumping { get; set;}
    #endregion

    #region Ability: Collision
    public Collider2D LastGroundCollided { get; set; }
    public bool IsGrounded { get; set; }
    public Action OnGroundReleased;
    public Action OnGroundLanded;

    #endregion

    #region Ability: Launch
    public Action OnLaunchStarted;
    //public Action OnLaunchEnded;
    #endregion

    #region Implementation: HealthModule
    public Action<DamageContainer> OnDamageTaken;
    private Coroutine iceStateCoroutine;
    private void InternalOnDamageTaken(DamageContainer damage)
    {
        StartCoroutine(StopInputCoroutine(timerInputDisableAfterHit));
        switch (damage.DamageType)
        {
            case DamageType.NormalDamage:
                Debug.Log("Player ha preso Normal Damage: " + damage.DamageAmount);
                break;
            case DamageType.FireDamage:
                Debug.Log("Player ha preso Fire Damage: " + damage.DamageAmount);
                break;
            case DamageType.IceDamage:
                Debug.Log("Player ha preso Ice Damage: " + damage.DamageAmount);
                if (iceStateCoroutine != null)
                {
                    StopCoroutine(iceStateCoroutine);
                    OnIceStateExited?.Invoke();
                }
                iceStateCoroutine = StartCoroutine(IceStateCoroutine());
                break;
            case DamageType.PoisonDamage:
                Debug.Log("Player ha preso Poison Damage: " + damage.DamageAmount);
                break;
        }
    }
    #endregion

    #region InternalMethods
    private void DisableInputs()
    {
        foreach (var ability in abilities)
        {
            ability.OnInputDisable();
        }
    }
    private void EnableInputs()
    {
        foreach (var ability in abilities)
        {
            ability.OnInputEnable();
        }
    }
    private IEnumerator StopInputCoroutine(float freezeTime)
    {
        DisableInputs();
        yield return new WaitForSeconds(freezeTime);
        EnableInputs();
    }
    private IEnumerator IceStateCoroutine()
    {
        OnIceStateEntered?.Invoke();
        yield return new WaitForSeconds(10);
        OnIceStateExited?.Invoke(); 
    }
    #endregion

    #region Public Methods

    #endregion

    #region DebugLogAbilities
    /*Inscribe all actions to a simple debug.Log to see if they will correctly called*/
    private void DebugAbilities()
    {
        OnWalkStarted += ()=> Debug.Log("OnWalkStarted Called");
        OnWalkEnded += () => Debug.Log("OnWalkEnded Called");
        OnGroundLanded += () => Debug.Log("OnGroundLanded Called");
        OnGroundReleased += () => Debug.Log("OnGroundReleased Called");
        OnLaunchStarted += () => Debug.Log("OnLaunchStarted Called");
        //OnLaunchEnded += () => Debug.Log("OnLaunchEnded Called");
    }
    #endregion
}
