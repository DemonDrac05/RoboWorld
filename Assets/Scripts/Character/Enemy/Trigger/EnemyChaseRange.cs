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
            Debug.Log("Player in chase zone");
        }
    }

    private void OnTriggerExit(Collider triggerCollision)
    {
        if (triggerCollision.gameObject.CompareTag("Player"))
        {
            enemy.chaseRangeTrigger = false;
            Debug.Log("Player out chase zone");
        }
    }
}
