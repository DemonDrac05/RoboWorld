using UnityEngine;

public class EnemyChaseRange : MonoBehaviour
{
    private Enemy enemy;

    private void Awake() => enemy = GetComponentInParent<Enemy>();

    private void OnTriggerEnter(Collider triggerCollision)
    {
        if (triggerCollision.gameObject.CompareTag("Player"))
        {
            enemy.chaseRangeTrigger = true;
        }
    }

    private void OnTriggerExit(Collider triggerCollision)
    {
        if (triggerCollision.gameObject.CompareTag("Player"))
        {
            enemy.chaseRangeTrigger = false;
        }
    }
}
