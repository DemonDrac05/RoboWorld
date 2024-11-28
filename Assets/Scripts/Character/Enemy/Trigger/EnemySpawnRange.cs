using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnRange : MonoBehaviour
{
    private CyberMancubus cyberMancubus;
    private void Awake() => cyberMancubus = GetComponentInParent<CyberMancubus>();

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            cyberMancubus.spawnRangeTrigger = true;
        }
    }
}
