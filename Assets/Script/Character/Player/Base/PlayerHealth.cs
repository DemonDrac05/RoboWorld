using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("=== Health GUI Components ==========")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMPro.TextMeshProUGUI healthText;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentShield;

    private PlayerStat playerStat;

    private void Awake() => playerStat = GetComponent<PlayerStat>();

    private void Start()
    {
        if (playerStat != null)
        {
            currentHealth = healthSlider.maxValue = playerStat.Health;
            currentShield = playerStat.Shield;
        }
    }

    private void Update()
    {
        currentHealth = playerStat.Health;
        currentShield = playerStat.Shield;
        healthSlider.value = currentHealth;

        float healthPercent = healthSlider.value * 100f / playerStat.baseStatSO.maxHealth;
        healthText.text = $"{((int)healthPercent)}%";
    }
}
