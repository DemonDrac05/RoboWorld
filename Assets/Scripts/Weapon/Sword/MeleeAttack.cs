using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private string targetTag;

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.CompareTag(targetTag))
        {
            var defenderStat = trigger.GetComponent<BaseStatManager>();
            var attackerStat = this.GetComponentInParent<BaseStatManager>();

            if (defenderStat != null && attackerStat != null)
            {
                CombatManager.instance.ApplyMeleeDamage(attackerStat, defenderStat);
            }
        }
    }
}
