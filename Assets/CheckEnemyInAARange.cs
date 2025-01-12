using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyInAARange : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public List<Enemy> enemiesToRemove = new List<Enemy>();

    private void Update()
    {
        if (enemiesToRemove.Count > 0)
        {
            foreach (var enemy in enemiesToRemove)
            {
                enemies.Remove(enemy);
            }
            enemiesToRemove.Clear();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null && !enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null && enemies.Contains(enemy))
            {
                enemiesToRemove.Add(enemy);
            }
        }
    }
}