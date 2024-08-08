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
    #endregion

    #region Properties
    public Transform PlayerTransform { get { return playerTransform; } }
    public Rigidbody2D PlayerRigidBody { get {  return playerRigidBody; } }
    #endregion

    private void Start()
    {
        abilities = GetComponentsInChildren<PlayerAbilityBase>();
        foreach (PlayerAbilityBase ability in abilities)
        {
            ability.Init(this);
            ability.enabled = true;
        }
    }


}
