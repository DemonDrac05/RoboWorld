using Game.Stats;
using UnityEngine;

public class BaseStatManager : MonoBehaviour
{
    public BaseStatSO baseStatSO;

    public float Health { get; private set; }
    public float Shield { get; private set; }
    public float Defense { get; private set; }
    public float RangeWeaponDamage { get; private set; }
    public float MeleeWeaponDamage { get; private set; }
    public float RangeWeaponSpeed { get; private set; }
    public float MeleeWeaponSpeed { get; private set; }
    public float PiercingPercentage { get; private set; }
    public float CriticalChance { get; private set; }
    public float CriticalDamage { get; private set; }

    public virtual void OnEnable() => InitializeStat();

    public virtual void InitializeStat()
    {
        Health = baseStatSO.maxHealth;
        Shield = baseStatSO.maxShield;
        Defense = baseStatSO.maxDefense;

        RangeWeaponDamage = baseStatSO.rangeWeaponDamage;
        MeleeWeaponDamage = baseStatSO.meleeWeaponDamage;

        RangeWeaponSpeed = baseStatSO.rangeWeaponSpeed;
        MeleeWeaponSpeed = baseStatSO.meleeWeaponSpeed;

        PiercingPercentage = baseStatSO.piercingPercentage;
        CriticalChance = baseStatSO.criticalChance;
        CriticalDamage = baseStatSO.criticalDamage;
    }

    protected void SetHealth(float val)
    {
        Health = Mathf.Clamp(val, 0, baseStatSO.maxShield);
    }
    protected void SetShield(float val)
    {
        Shield = Mathf.Clamp(val, 0, baseStatSO.maxShield);
    }

    public void TakeHealthDamage(float damage)
    {
        Health = Mathf.Max(0f, Health - damage);
    }
    public void TakeShieldDamage(float damage)
    {
        Shield = Mathf.Max(0f, Shield - damage);
    }
}
