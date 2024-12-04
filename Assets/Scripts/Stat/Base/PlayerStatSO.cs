using Game.Stats;
using UnityEngine;

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

    [Header("=== Unique Stats Addition ==========")]
    [Range(0f, 1f)] public float shieldRecPercentage;
    [Range(0f, 1f)] public float shieldRecDuration;

    [Range(0f, 1f)] public float staminaRecPercentage;
    [Range(0f, 1f)] public float staminaRecDuration;
}