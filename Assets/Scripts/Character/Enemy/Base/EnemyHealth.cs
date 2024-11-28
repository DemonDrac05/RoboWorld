using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("=== Health GUI Components ==========")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider shieldSlider;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentShield;

    private EnemyStat enemyStat;

    private void Awake() => enemyStat = GetComponent<EnemyStat>();

    private void Start()
    {
        if (enemyStat != null)
        {
            currentHealth = healthSlider.maxValue = enemyStat.Health;
            currentShield = shieldSlider.maxValue = enemyStat.Shield;
        }
    }

    private void Update()
    {
        currentHealth = enemyStat.Health;
        currentShield = enemyStat.Shield;

        healthSlider.value = currentHealth;
        shieldSlider.value = currentShield;

        if (currentHealth <= 0f) Destroy(this.gameObject);
    }
}
