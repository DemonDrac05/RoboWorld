using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    private void Awake() => instance = this;

    public void ApplyRangedDamage(BaseStatManager attacker, BaseStatManager defender)
        => ApplyDamage(attacker, defender, AttackType.Range);

    public void ApplyMeleeDamage(BaseStatManager attacker, BaseStatManager defender)
        => ApplyDamage(attacker, defender, AttackType.Melee);

    public void ApplyMissileDamage(PlayerStat playerStat, BaseStatManager defender)
        => ApplyDamage(playerStat, defender, AttackType.Missile);

    public void ApplyDamage(BaseStatManager attacker, BaseStatManager defender, AttackType type)
    {
        float typeOfAttackDamage = type switch
        {
            AttackType.Range => attacker.RangeWeaponDamage,
            AttackType.Melee => attacker.MeleeWeaponDamage,
            AttackType.Missile => CalculateMissileDamage(attacker),
            _ => 0f,
        };


        float totalDamage = CalculateCriticalDamage(attacker, typeOfAttackDamage);
        totalDamage = type != AttackType.Missile 
                        ? CalculatePiercingDamage(attacker, totalDamage, defender.Defense) : totalDamage;

        if (defender.Shield > 0f)
        {
            var totalDamageToShield = CalculateCriticalDamage(attacker, typeOfAttackDamage) - defender.Shield;

            if (totalDamageToShield >= 0f)
            {
                defender.TakeShieldDamage(defender.Shield);
                defender.TakeHealthDamage(totalDamageToShield - defender.Defense);
            }
            else if (totalDamageToShield < 0f)
            {
                defender.TakeShieldDamage(typeOfAttackDamage);
            }
        }
        else
        {
            defender.TakeHealthDamage(Mathf.Max(0f, totalDamage));
        }
    }

    private float CalculateCriticalDamage(BaseStatManager attacker, float baseDamage)
    {
        return Random.Range(0f, 1f) < attacker.CriticalChance ? baseDamage * attacker.CriticalDamage : baseDamage;
    }

    private float CalculatePiercingDamage(BaseStatManager attacker, float baseDamage, float defenderDefense)
    {
        return baseDamage - (1 - attacker.PiercingPercentage) * defenderDefense;
    }

    private static float CalculateMissileDamage(BaseStatManager attacker)
    {
        return attacker is PlayerStat stat ? stat.MissileDamage : 0f;
    }
}

public enum AttackType
{
    //Basic Type
    Melee, Range,

    //Unique Type
    Missile
}