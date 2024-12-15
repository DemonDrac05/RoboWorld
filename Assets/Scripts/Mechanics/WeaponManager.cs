using System.Collections;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    private void Awake() => instance = this;

    public void FireBullet(GameObject bulletPrefab, Transform shootingPoint, float bulletSpeed)
    {
        var bullet = Instantiate(bulletPrefab, shootingPoint.transform.position, shootingPoint.rotation);

        bullet.GetComponent<Bullet>().Initialize(shootingPoint.gameObject);
        bullet.GetComponent<Rigidbody>().linearVelocity = shootingPoint.forward * bulletSpeed;
    }
}
