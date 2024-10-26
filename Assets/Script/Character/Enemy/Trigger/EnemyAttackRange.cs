using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    private Enemy enemy;

    private void Awake() => enemy = GetComponentInParent<Enemy>();

    private void OnTriggerEnter(Collider triggerCollision)
    {
        if (triggerCollision.gameObject.CompareTag("Player"))
        {
            enemy.attackRangeTrigger = true;
        }
    }

    private void OnTriggerExit(Collider triggerCollision)
    {
        if (triggerCollision.gameObject.CompareTag("Player"))
        {
            enemy.attackRangeTrigger = false;
        }
    }
}
