using UnityEngine;

namespace Game.Stats
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Stats/Base Stats")]
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

        [Header("=== Unique Stat ==========")]
        [Range(0f, 1f)] public float piercingPercentage;

        [Range(0f, 1f)] public float criticalChance;
        [Range(0f, 10f)] public float criticalDamage;
    }
}


