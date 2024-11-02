using UnityEngine;

public class MissilePrelaunchRange : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            if (!MissileLaunch.missileManager.enemyList.ContainsKey(collider.gameObject)
                && MissileLaunch.missileManager.isPreLaunching)
            {
                MissileLaunch.missileManager.enemyList.Add(collider.gameObject, 0f);
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            if (MissileLaunch.missileManager.enemyList.ContainsKey(collider.gameObject)
                && MissileLaunch.missileManager.isPreLaunching)
            {
                MissileLaunch.missileManager.enemyList.Remove(collider.gameObject);
                if (MissileLaunch.missileManager.enemyList.Count == 0)
                {
                    MissileLaunch.missileManager.currentMissileQuantity = 0;
                }
            }
        }
    }
}
