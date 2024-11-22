using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    private void Awake() => instance = this;

    public void ApplyRangedDamage(BaseStatManager attacker, BaseStatManager defender)
    {
        var totalRangedDamage = attacker.rangeWeaponDamage - defender.Defense;
        if (totalRangedDamage < 0f)
        {
            totalRangedDamage = -totalRangedDamage;
        }
        if (defender.Shield > 0f)
        {
            var totalDamageToShield = attacker.rangeWeaponDamage - defender.Shield;

            if (totalDamageToShield >= 0f)
            {
                defender.Shield = 0f;
                defender.Health -= (totalDamageToShield - defender.Defense);
            }
            else if (totalDamageToShield < 0f)
            {
                defender.Shield += totalDamageToShield;
            }
        }
        else
        {
            defender.Health -= totalRangedDamage;
        }
    }
}
