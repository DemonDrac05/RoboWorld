using Game.Stats;
using UnityEngine;

public class BaseStatManager : MonoBehaviour
{
    public BaseStatSO baseStatSO;

    public float Speed { get; private set; }
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
        Speed = baseStatSO.maxSpeed;
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

    protected void SetSpeed(float val) => Speed = Mathf.Clamp(val, 0f, baseStatSO.maxSpeed);
    protected void SetHealth(float val) => Health = Mathf.Clamp(val, 0, baseStatSO.maxHealth);
    protected void SetShield(float val) => Shield = Mathf.Clamp(val, 0, baseStatSO.maxShield);
    protected void SetDefense(float val) => Defense = Mathf.Clamp(val, 0, baseStatSO.maxDefense);
    public void SetRangeWeaponDamage(float val) => RangeWeaponDamage = Mathf.Clamp(val, 0, baseStatSO.rangeWeaponDamage);
    public void SetMeleeWeaponDamage(float val) => MeleeWeaponDamage = Mathf.Clamp(val, 0, baseStatSO.meleeWeaponDamage);
    public void SetRangeWeaponSpeed(float val) => RangeWeaponSpeed = Mathf.Clamp(val, 0, baseStatSO.rangeWeaponSpeed);
    public void SetMeleeWeaponSpeed(float val) => MeleeWeaponSpeed = Mathf.Clamp(val, 0, baseStatSO.meleeWeaponSpeed);
    public void SetPiercingPercentage(float val) => PiercingPercentage = Mathf.Clamp(val, 0, baseStatSO.piercingPercentage);
    public void SetCriticalChance(float val) => CriticalChance = Mathf.Clamp(val, 0, baseStatSO.criticalChance);
    public void SetCriticalDamage(float val) => CriticalDamage = Mathf.Clamp(val, 0, baseStatSO.criticalDamage);

    public void TakeHealthDamage(float damage)
    {
        Health = Mathf.Max(0f, Health - damage);
    }
    public void TakeShieldDamage(float damage)
    {
        Shield = Mathf.Max(0f, Shield - damage);
    }
}
