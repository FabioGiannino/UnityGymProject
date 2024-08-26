using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField]
    private DamageContainer damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>() != null ? collision.GetComponent<IDamageable>() : collision.GetComponentInChildren<IDamageable>();
        if(damageable == null) return;

        damageable.TakeDamage(damage);

    }
}
