using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("=== Health GUI Components ==========")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMPro.TextMeshProUGUI healthText;
    [SerializeField] private TMPro.TextMeshProUGUI maxHealthText;

    [Header("=== Shield GUI Components ==========")]
    [SerializeField] private Slider shieldSlider;

    [Header("=== Stamina GUI Components ==========")]
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private TMPro.TextMeshProUGUI staminaText;
    [SerializeField] private TMPro.TextMeshProUGUI maxStaminaText;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentShield;
    [HideInInspector] public float currentStamina;

    private float shieldRecTimer = 0f;
    private float staminaRecTimer = 0f;

    private PlayerStat playerStat;

    private void Awake() => playerStat = GetComponent<PlayerStat>();

    private void Start()
    {
        if (playerStat != null)
        {
            InitializeStatUI(ref currentHealth, healthSlider, maxHealthText, playerStat.Health);
            InitializeStatUI(ref currentShield, shieldSlider, null, playerStat.Shield);
            InitializeStatUI(ref currentStamina, staminaSlider, maxStaminaText, playerStat.Stamina);
        }
    }

    private void InitializeStatUI(ref float currentValue, Slider slider, TMPro.TextMeshProUGUI maxText, float maxValue)
    {
        currentValue = slider.maxValue = maxValue;
        if(maxText != null) maxText.text = $"{maxValue}";
    }

    private void Update()
    {
        currentHealth = playerStat.Health;
        currentShield = playerStat.Shield;
        currentStamina = playerStat.Stamina;

        UpdateStatUI(healthSlider, healthText, currentHealth);
        UpdateStatUI(shieldSlider, null, currentShield);
        UpdateStatUI(staminaSlider, staminaText, currentStamina);

        ShieldRecovery();
        StaminaRecovery();
    }

    private void UpdateStatUI(Slider slider, TMPro.TextMeshProUGUI text, float currentValue)
    {
        slider.value = currentValue;
        if (text != null) text.text = $"{currentValue}";
    }

    private void RecoverStat(ref float currentValue, float maxValue, float recoveryRate, float recoveryDuration, ref float timer, System.Action<float> applyRecovery)
    {
        if (currentValue < maxValue)
        {
            timer += Time.deltaTime;
            if (timer >= recoveryDuration)
            {
                float recoveryAmount = recoveryRate * maxValue;
                applyRecovery(recoveryAmount);
                timer = 0f;
            }
        }
        else
        {
            applyRecovery(0);
            timer = 0f;
        }
    }

    void ShieldRecovery()
    {
        RecoverStat(ref currentShield,
            playerStat.playerStatSO.maxShield,
            playerStat.ShieldRecPercentage,
            playerStat.ShieldRecDuration,
            ref shieldRecTimer,
            playerStat.RecoveryShield);
    }

    void StaminaRecovery()
    {
        RecoverStat(ref currentStamina,
            playerStat.playerStatSO.maxStamina,
            playerStat.StaminaRecPercentage,
            playerStat.StaminaRecDuration,
            ref staminaRecTimer,
            playerStat.RecoveryStamina);
    }
}
