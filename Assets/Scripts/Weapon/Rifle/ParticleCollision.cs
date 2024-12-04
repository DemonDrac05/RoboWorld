using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    private ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public GameObject explosionPrefab;

    private void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject collision)
    {
        int numCollisionEvents = part.GetCollisionEvents(collision, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Vector3 collisionPoint = collisionEvents[i].intersection;
            Instantiate(explosionPrefab, collisionPoint, Quaternion.identity);

            var enemyStat = collision.GetComponent<EnemyStat>();
            var playerStat = collision.GetComponent<PlayerStat>();

            if (enemyStat != null)
            {
                var player = this.GetComponentInParent<PlayerStat>();
                if (player != null)
                {
                    CombatManager.instance.ApplyRangedDamage(player, enemyStat);
                }
            }
            else if (playerStat != null)
            {
                var enemy = this.GetComponentInParent<EnemyStat>();
                if (enemy != null)
                {
                    CombatManager.instance.ApplyRangedDamage(enemy, playerStat);
                }
            }
        }
    }
}
