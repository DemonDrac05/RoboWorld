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
        //    Debug.Log(collisionEvents[0].intersection);
        //}

        var getPlayerStat = this.GetComponentInParent<PlayerStat>();
        var getEnemyStat = this.GetComponentInParent<EnemyStat>();

        if (collision.GetComponent<Collider>() == Player.player.collider && getEnemyStat != null)
        {
            getPlayerStat = PlayerStat.playerStat;
            RangeDamage(getEnemyStat, getPlayerStat);
        }
        if (collision.GetComponent<Collider>() == Enemy.enemy.collider && getPlayerStat != null)
        {
            getEnemyStat = EnemyStat.enemyStat;
            RangeDamage(getPlayerStat, getEnemyStat);
        }
    }

    private void RangeDamage(BaseStat attacker, BaseStat defender)
    {
        var totalDamage = attacker.rangeWeaponDamage - defender.defense;
        if (defender.shield > 0f)
        {
            totalDamage = attacker.rangeWeaponDamage - defender.shield;
            if (totalDamage > 0f)
            {
                defender.shield = 0f;
                defender.health -= (totalDamage - defender.defense);
                if (defender.health > defender.maxHealth)
                {
                    defender.health = defender.maxHealth;
                }
            }
            else
            {
                defender.shield = -totalDamage;
            }
        }
        else
        {
            defender.health -= totalDamage;
        }
    }
}
