using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Stat")]
public class BaseStatSO : ScriptableObject
{
    [Header("=== Basic Stat ==========")]
    public float maxHealth;
    public float maxShield;
    public float maxDefense;

    [Header("=== Weapon Damage ==========")]
    public float rangeWeaponDamage;
    public float meleeWeaponDamage;

    [Header("=== Weapon Speed ==========")]
    public float rangeWeaponSpeed;
    public float meleeWeaponSpeed;
}
