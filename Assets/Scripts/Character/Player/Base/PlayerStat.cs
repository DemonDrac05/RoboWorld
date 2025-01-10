using Cysharp.Threading.Tasks;
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
    }

    public void SetStamina(float val) => Stamina = Mathf.Clamp(val, 0, playerStatSO.maxStamina);
    public void RecoveryShield(float amount) => SetShield(Shield + amount);
    public void RecoveryStamina(float amount) => Stamina = Mathf.Min(Stamina + amount, playerStatSO.maxStamina);

    public PlayerData CreatePlayerData(Vector3 saveCheckpoint) => new PlayerData(this, saveCheckpoint);

    public async UniTask ApplyPlayerData(PlayerData playerData)
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
        transform.position = Checkpoint = new Vector3(playerData.checkpoint[0], playerData.checkpoint[1], playerData.checkpoint[2]);

        await UniTask.Yield();
    }
}
