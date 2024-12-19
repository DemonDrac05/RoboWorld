using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private BaseStat playerStat;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentShield;
    [HideInInspector] public static PlayerHealth player;

    private void Awake()
    {
        player = this;

        currentHealth = playerStat.maxHealth;
        currentShield = playerStat.maxShield;
    }
}
