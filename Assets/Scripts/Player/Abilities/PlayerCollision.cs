using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : PlayerAbilityBase
{
    #region SerializeFields
    [SerializeField]
    protected LayerMask groundLayer;
    [SerializeField]
    protected LayerMask wallLayer;
    [SerializeField]
    protected CollisionSphereData[] spheres;
    #endregion

    #region Protected Properties
    protected Collider2D LastGroundCollider
    {
        get { return playerController.LastGroundCollided; }
        set { playerController.LastGroundCollided = value; }
    }
    #endregion

    #region  Override
    public override void OnInputDisable()
    {
    }

    public override void OnInputEnable()
    {
    }

    public override void StopAbility()
    {
    }
    #endregion

    #region Internal Classes
    public enum ColliderPointPosition
    {
        Center = 0,
        BottomCenter = 1,
        LeftCenter = 2,
        RightCenter = 3,
    }


    [System.Serializable]
    protected class CollisionSphereData
    {
        [SerializeField]
        private ColliderPointPosition pointPosition;
        [SerializeField]
        private float radius;
        [SerializeField]
        private Vector2 offset;
        [Space]
        [Header("Editor Only")]
        [SerializeField]
        private Color color;

        public ColliderPointPosition PointPosition { get { return pointPosition; } }
        public float Radius { get { return radius; } }
        public Vector2 Offset { get { return offset; } }
        public Color Color { get { return color; } }
    }
    #endregion

    

    #region Mono
    protected void Update()
    {
        DetectGroundCollision();
    }
    #endregion

    #region Gizmos per l'editor
    /// <summary>
    /// Crea dei Gizmos, SARà ESEGUITO DA EDITOR, NON IN GAME
    /// </summary>
    protected void OnDrawGizmos()
    {
        if (playerController == null)
        {
            playerController = GameObject.FindObjectOfType<PlayerController>();
        }
        DrawSphere();

    }
    protected void DrawSphere()
    {
        foreach (var sphere in spheres)
        {
            Gizmos.color = sphere.Color;
            Vector2 point = GetCollisionPoint(sphere.PointPosition);
            float radius = GetSphereRadius(sphere.PointPosition);
            Gizmos.DrawWireSphere(point, radius);
        }
    }
    #endregion

    #region Interal Methods
    protected Vector2 GetCollisionPoint(ColliderPointPosition position)
    {
        CollisionSphereData data = System.Array.Find(spheres, spheres => spheres.PointPosition == position);
        Vector2 positionOffset = data == null ? Vector2.zero : data.Offset;
        Vector2 collisionPoint = Vector2.zero;

        //se non esiste un collider fisico del player, ritorno il vector0 + l'offset
        if (playerController.PlayerPhysicsCollider == null) return collisionPoint + positionOffset;

        Vector2 playerCenter = playerController.PlayerPhysicsCollider.bounds.center;    //il centro del collider2d impostato
        Vector2 playerExtents = playerController.PlayerPhysicsCollider.bounds.extents;  //ritorna la bounding BOX, il quadrato che contiene il collider

        switch (position)
        {
            case ColliderPointPosition.BottomCenter:
                collisionPoint = new Vector2(playerCenter.x, playerCenter.y - playerExtents.y);
                break;
            case ColliderPointPosition.LeftCenter:
                collisionPoint = new Vector2(playerCenter.x - playerExtents.x, playerCenter.y);
                break;
            case ColliderPointPosition.RightCenter:
                collisionPoint = new Vector2(playerCenter.x + playerExtents.x, playerCenter.y);
                break;
            case ColliderPointPosition.Center:
                collisionPoint = playerCenter;
                break;
        }
        return collisionPoint + positionOffset;

    }
    protected float GetSphereRadius(ColliderPointPosition position)
    {
        CollisionSphereData data = System.Array.Find(spheres, spheres => spheres.PointPosition == position);
        if (data == null) return -1;
        return data.Radius;
    }

    protected void DetectGroundCollision()
    {
        bool wasGrounded = playerController.IsGrounded;
        Vector2 collisionPoint = GetCollisionPoint(ColliderPointPosition.BottomCenter);
        float sphereRadius = GetSphereRadius(ColliderPointPosition.BottomCenter);

        LastGroundCollider = Physics2D.OverlapCircle(collisionPoint, sphereRadius, groundLayer);
        playerController.IsGrounded = LastGroundCollider != null;


        if (wasGrounded == playerController.IsGrounded) return;
        if (wasGrounded)
        {
            playerController.OnGroundReleased?.Invoke();
        }
        else
        {
            playerController.OnGroundLanded?.Invoke();
        }
    }
    #endregion

}