using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    private void Awake() => instance = this;

    public void ApplyRangedDamage(BaseStatManager attacker, BaseStatManager defender)
    {
        var totalRangedDamage = attacker.RangeWeaponDamage - defender.Defense;

        if (attacker is PlayerStat stat)
        {
            var piercingDamage = stat.PiercingPercentage * stat.RangeWeaponDamage;
            defender.TakeHealthDamage(piercingDamage);
            totalRangedDamage -= piercingDamage;
        }

        if (defender.Shield > 0f)
        {
            var totalDamageToShield = attacker.RangeWeaponDamage - defender.Shield;

            if (totalDamageToShield >= 0f)
            {
                defender.TakeShieldDamge(defender.Shield);
                defender.TakeHealthDamage(totalDamageToShield - defender.Defense);
            }
            else if (totalDamageToShield < 0f)
            {
                defender.TakeShieldDamge(attacker.RangeWeaponDamage);
            }
        }
        else
        {
            defender.TakeHealthDamage(Mathf.Max(0f, totalRangedDamage));
        }
    }
}