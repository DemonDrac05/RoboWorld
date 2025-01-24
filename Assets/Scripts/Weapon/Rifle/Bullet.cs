using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float existingTime; 
    // ---
    private EnemyStat cachedEnemyStat;
    private PlayerStat cachedPlayerStat;
    // ---
    private GameObject bulletParent;

    public void Initialize(GameObject parent) => bulletParent = parent;

    private void Update() => Destroy(gameObject, existingTime);

    private void OnCollisionEnter(Collision collision)
    {
        cachedEnemyStat = collision.gameObject.GetComponent<EnemyStat>();
        cachedPlayerStat = collision.gameObject.GetComponent<PlayerStat>();

        if (cachedEnemyStat != null)
        {
            HandleEnemyCollision(cachedEnemyStat);
        }
        else if (cachedPlayerStat != null)
        {
            HandlePlayerCollision(cachedPlayerStat);
        }

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void HandleEnemyCollision(EnemyStat enemyStat)
    {
        enemyStat.TakeHealthDamage(50f);
    }

    private void HandlePlayerCollision(PlayerStat playerStat)
    {
        playerStat.TakeHealthDamage(1f);
    }
}
