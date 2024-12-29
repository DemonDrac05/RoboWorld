using Game.Stats;
using UnityEngine;

public class PlayerStat : BaseStatManager
{
    public PlayerStatSO playerStatSO;

    public static PlayerStat playerStat { get; private set; }

    //Properties for runtime values
    // --- STAT -----------
    public float Stamina { get; private set; }
    public float MissileDamage { get; private set; }
    public float MissileAOEDamage { get; private set; }
    public float MissileLaunchingTime { get; private set; }
    public float MissileLaunchingCooldown { get; private set; }
    public float ShieldRecPercentage { get; private set; }
    public float ShieldRecDuration { get; private set; }
    public float StaminaRecPercentage { get; private set; }
    public float StaminaRecDuration { get; private set; }
    // --- PROPERTIES ----------
    public Vector3 Checkpoint { get; private set; }
    public float HeightOfCheckpoint { get; private set; }

    public override void OnEnable()
    {
        playerStat = this;
        ApplyPlayerData(SaveSystem.LoadGame());
    }

    public override void InitializeStat()
    {
        base.InitializeStat();
        
        Stamina = playerStatSO.maxStamina;

        MissileDamage = playerStatSO.missileDamage;
        MissileAOEDamage = playerStatSO.missileAOEDamage;

        MissileLaunchingTime = playerStatSO.missileLaunchingTime;
        MissileLaunchingCooldown = playerStatSO.missileLaunchingCooldown;

        ShieldRecPercentage = playerStatSO.shieldRecPercentage;
        ShieldRecDuration = playerStatSO.shieldRecDuration;
        StaminaRecPercentage = playerStatSO.staminaRecPercentage;
        StaminaRecDuration = playerStatSO.staminaRecDuration;

        Checkpoint = playerStatSO.checkPoint;
        HeightOfCheckpoint = playerStatSO.heightOfCheckpoint;
    }

    public void SetStamina(float val) => Stamina = Mathf.Clamp(val, 0, playerStatSO.maxStamina);
    public void RecoveryShield(float amount) => SetShield(Shield + amount);
    public void RecoveryStamina(float amount) => Stamina = Mathf.Min(Stamina + amount, playerStatSO.maxStamina);

    public void SetCheckpoint(Vector3 savedCheckpoint)
    {
        Vector3 fixedCheckpoint = new Vector3(savedCheckpoint.x, savedCheckpoint.y + HeightOfCheckpoint, savedCheckpoint.z);
        playerStatSO.checkPoint = Checkpoint = fixedCheckpoint;
    }

    public PlayerData CreatePlayerData(Vector3 saveCheckpoint) => new PlayerData(this, saveCheckpoint);

    public void ApplyPlayerData(PlayerData playerData)
    {
        SetSpeed(playerData.maxSpeed);
        SetHealth(playerData.maxHealth);
        SetShield(playerData.maxShield);
        SetDefense(playerData.maxDefense);
        SetStamina(playerData.maxStamina);

        SetRangeWeaponDamage(playerData.rangeWeaponDamage);
        SetMeleeWeaponDamage(playerData.meleeWeaponDamage);
        SetRangeWeaponSpeed(playerData.rangeWeaponSpeed);
        SetMeleeWeaponSpeed(playerData.meleeWeaponSpeed);

        SetPiercingPercentage(playerData.piercingPercentage);
        SetCriticalChance(playerData.criticalChance);
        SetCriticalDamage(playerData.criticalDamage);

        MissileDamage = playerData.missileDamage;
        MissileAOEDamage = playerData.missileAOEDamage;
        MissileLaunchingTime = playerData.missileLaunchingTime;
        MissileLaunchingCooldown = playerData.missileLaunchingCooldown;

        ShieldRecPercentage = playerData.shieldRecPercentage;
        ShieldRecDuration = playerData.shieldRecDuration;
        StaminaRecPercentage = playerData.staminaRecPercentage;
        StaminaRecDuration = playerData.staminaRecDuration;

        HeightOfCheckpoint = playerData.heightOfCheckpoint;
        Checkpoint = new Vector3(playerData.checkpoint[0], playerData.checkpoint[1], playerData.checkpoint[2]);
    }
}
