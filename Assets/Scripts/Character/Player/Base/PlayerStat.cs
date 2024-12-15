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
        InitializeStat();
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
}
