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

    [CreateAssetMenu(menuName = "Stats/Player Stats")]
    public class PlayerStatSO : BaseStatSO
    {
        [Header("=== Basic Addition =========")]
        public float maxStamina;

        [Header("=== Weapon Damage Addition ==========")]
        public float missileDamage;
        public float missileAOEDamage;

        [Header("=== Weapon Speed Addition ==========")]
        public float missileLaunchingTime;
        public float missileLaunchingCooldown;

        [Header("=== Unique Stats ==========")]
        [Range(0f, 1f)] public float piercingPercentage;

        [Range(0f, 1f)] public float shieldRecPercentage;
        [Range(0f, 1f)] public float shieldRecDuration;

        [Range(0f, 1f)] public float staminaRecPercentage;
        [Range(0f, 1f)] public float staminaRecDuration;
    }
}


