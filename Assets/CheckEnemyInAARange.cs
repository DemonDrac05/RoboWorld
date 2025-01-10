using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyInAARange : MonoBehaviour
{
    public List<Vector3> enemies = new List<Vector3>();
    public List<Vector3> enemyToRemove = new List<Vector3>();

    private void Update()
    {
        if (enemyToRemove.Count > 0)
        {
            foreach (var enemy in enemyToRemove)
            {
                enemies.Remove(enemy);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            var enemyPos = GetComponent<Enemy>().transform.position;
            if (!enemies.Contains(enemyPos))
            {
                enemies.Add(enemyPos);
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            var enemyPos = GetComponent<Enemy>().transform.position;
            if (enemies.Contains(enemyPos))
            {
                enemyToRemove.Add(enemyPos);
            }
        }
    }
}
