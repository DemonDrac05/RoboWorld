using UnityEngine;

public class EnemyShootRange : MonoBehaviour
{
    private Enemy enemy;

    private void Awake() => enemy = GetComponentInParent<Enemy>();

    private void OnTriggerEnter(Collider triggerCollision)
    {
        if (triggerCollision.gameObject.CompareTag("Player"))
        {
            enemy.shootRangeTrigger = true;
        }
    }

    private void OnTriggerExit(Collider triggerCollision)
    {
        if (triggerCollision.gameObject.CompareTag("Player"))
        {
            enemy.shootRangeTrigger = false;
        }
    }
}
