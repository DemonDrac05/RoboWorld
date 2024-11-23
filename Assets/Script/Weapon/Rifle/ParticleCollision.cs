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

        GameObject explosion = Instantiate(explosionPrefab, collisionEvents[0].intersection, Quaternion.identity);

        //if (collision.GetComponent<Rigidbody>() != null)
        //{
        //    var rb = collision.GetComponent<Rigidbody>();
        //    Vector3 newPos = new(-rb.transform.position.x, rb.transform.position.y, -rb.transform.position.z);
        //    rb.AddForceAtPosition(collisionEvents[0].intersection * 10 - newPos, collisionEvents[0].intersection);
        //}

        Rigidbody collisionRb = collision.GetComponent<Rigidbody>();
        if (collisionRb == null) return;

        PlayerStat playerStat = GetComponentInParent<PlayerStat>();
        EnemyStat enemyStat = GetComponentInParent<EnemyStat>();

        if (collisionRb == Player.player.rb && enemyStat != null)
        {
            PlayerStat targetPlayerStat = collision.GetComponent<PlayerStat>();
            CombatManager.instance.ApplyRangedDamage(enemyStat, targetPlayerStat);
        }
        if (collisionRb == Enemy.enemy.rb && playerStat != null)
        {
            EnemyStat targetEnemyStat = collision.GetComponent<EnemyStat>();
            CombatManager.instance.ApplyRangedDamage(playerStat, targetEnemyStat);
        }
    }
}
