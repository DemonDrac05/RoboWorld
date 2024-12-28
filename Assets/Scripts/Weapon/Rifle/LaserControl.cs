using Unity.VisualScripting;
using UnityEngine;

public class LaserControl : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var bullet = Instantiate(bulletPrefab, shootingPoint.transform.position, shootingPoint.rotation);

            bullet.GetComponent<Bullet>().Initialize(shootingPoint.gameObject);
            bullet.GetComponent<Rigidbody>().linearVelocity = shootingPoint.forward * bulletSpeed;
        }
    }
}
