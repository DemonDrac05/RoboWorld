using UnityEngine;

public class BaseStatManager : MonoBehaviour
{
    public BaseStatSO baseStatSO;

    [Header("=== Basic Stat ==========")]
    public float Health;
    public float Shield;
    public float Defense;

    [Header("=== Weapon Damage ==========")]
    public float rangeWeaponDamage;
    public float meleeWeaponDamage;

    [Header("=== Weapon Speed ==========")]
    public float rangeWeaponSpeed;
    public float meleeWeaponSpeed;

    public virtual void Awake()
    {
        InitializeStat();
    }

    private void InitializeStat()
    {
        Health = baseStatSO.maxHealth;
        Shield = baseStatSO.maxShield;
        Defense = baseStatSO.maxDefense;

        rangeWeaponDamage = baseStatSO.rangeWeaponDamage;
        meleeWeaponDamage = baseStatSO.meleeWeaponDamage;

        rangeWeaponSpeed = baseStatSO.rangeWeaponSpeed;
        meleeWeaponSpeed = baseStatSO.meleeWeaponSpeed;
    }
}
