using System;
using UnityEngine;
public enum DamageType
{
    NormalDamage,
    FireDamage,
    IceDamage,
    PoisonDamage
}

[Serializable]
public class DamageContainer
{
    [SerializeField]
    private DamageType damageType;
    [SerializeField]
    private float damageAmount;

    public DamageType DamageType { get { return damageType; } }
    public float DamageAmount { get {  return damageAmount; } }

    public static DamageContainer DamageContainerFactory(DamageType damageType, float damageAmount)
    {
        DamageContainer damageContainer = new DamageContainer();
        damageContainer.damageType = damageType;
        damageContainer.damageAmount = damageAmount;
        return damageContainer;
    }

}
