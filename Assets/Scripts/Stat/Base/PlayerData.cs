using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float maxSpeed, maxHealth, maxShield, maxDefense, maxStamina;
    public float rangeWeaponDamage, meleeWeaponDamage;
    public float missileDamage, missileAOEDamage, missileLaunchingTime, missileLaunchingCooldown;
    public float rangeWeaponSpeed, meleeWeaponSpeed;
    public float piercingPercentage, criticalChance, criticalDamage;
    public float shieldRecPercentage, shieldRecDuration, staminaRecPercentage, staminaRecDuration;
    public float heightOfCheckpoint;
    public float[] checkpoint;

    /// <summary>
    /// Create new Player Data from default database from PlayerBase
    /// </summary>
    public PlayerData()
    {
        var playerBaseStat = Resources.Load<PlayerStatSO>("Statistic/PlayerBase");
        if (playerBaseStat != null)
        {
            maxSpeed = playerBaseStat.maxSpeed;
            maxHealth = playerBaseStat.maxHealth;
            maxShield = playerBaseStat.maxShield;
            maxDefense = playerBaseStat.maxDefense;
            maxStamina = playerBaseStat.maxStamina;

            rangeWeaponDamage = playerBaseStat.rangeWeaponDamage;
            meleeWeaponDamage = playerBaseStat.meleeWeaponDamage;
            rangeWeaponSpeed = playerBaseStat.rangeWeaponSpeed;
            meleeWeaponSpeed = playerBaseStat.meleeWeaponSpeed;

            missileDamage = playerBaseStat.missileDamage;
            missileAOEDamage = playerBaseStat.missileAOEDamage;
            missileLaunchingTime = playerBaseStat.missileLaunchingTime;
            missileLaunchingCooldown = playerBaseStat.missileLaunchingCooldown;

            piercingPercentage = playerBaseStat.piercingPercentage;
            criticalChance = playerBaseStat.criticalChance;
            criticalDamage = playerBaseStat.criticalDamage;
            shieldRecPercentage = playerBaseStat.shieldRecPercentage;
            shieldRecDuration = playerBaseStat.shieldRecDuration;
            staminaRecPercentage = playerBaseStat.staminaRecPercentage;
            staminaRecDuration = playerBaseStat.staminaRecDuration;

            heightOfCheckpoint = playerBaseStat.heightOfCheckpoint;

            Vector3 _defaultCheckpoint = Resources.Load<CheckpointSO>("Checkpoint/PlayerCheckpoint").checkpoints[0];
            checkpoint = new float[3]
            {
                _defaultCheckpoint.x,
                _defaultCheckpoint.y + heightOfCheckpoint,
                _defaultCheckpoint.z
            };
        }
        if (playerBaseStat == null) Debug.Log("Asset Unfound");
    }

    /// <summary>
    /// Create new Player Data from on-game database from real-time stat
    /// </summary>
    /// <param name="playerStat"></param>
    public PlayerData(PlayerStat playerStat, Vector3 saveCheckpoint)
    {
        maxSpeed = playerStat.Speed;
        maxHealth = playerStat.Health;
        maxShield = playerStat.Shield;
        maxDefense = playerStat.Defense;
        maxStamina = playerStat.Stamina;

        rangeWeaponDamage = playerStat.RangeWeaponDamage;
        meleeWeaponDamage = playerStat.MeleeWeaponDamage;

        rangeWeaponSpeed = playerStat.RangeWeaponSpeed;
        meleeWeaponSpeed = playerStat.MeleeWeaponSpeed;

        missileDamage = playerStat.MissileDamage;
        missileAOEDamage = playerStat.MissileAOEDamage;
        missileLaunchingTime = playerStat.MissileLaunchingTime;
        missileLaunchingCooldown = playerStat.MissileLaunchingCooldown;

        piercingPercentage = playerStat.PiercingPercentage;
        criticalChance = playerStat.CriticalChance;
        criticalDamage = playerStat.CriticalDamage;
        shieldRecPercentage = playerStat.ShieldRecPercentage;
        shieldRecDuration = playerStat.ShieldRecDuration;
        staminaRecPercentage = playerStat.StaminaRecPercentage;
        staminaRecDuration = playerStat.StaminaRecDuration;

        heightOfCheckpoint = playerStat.HeightOfCheckpoint;
        checkpoint = new float[3]
        {
            saveCheckpoint.x,
            saveCheckpoint.y + playerStat.HeightOfCheckpoint,
            saveCheckpoint.z
        };
    }
}
