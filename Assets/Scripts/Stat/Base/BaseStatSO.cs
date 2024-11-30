using UnityEngine;

namespace Game.Stats
{
    [CreateAssetMenu(menuName = "Stats/Base Stats")]
    public class BaseStatSO : ScriptableObject
    {
        [Header("=== Basic Stats ==========")]
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
}


