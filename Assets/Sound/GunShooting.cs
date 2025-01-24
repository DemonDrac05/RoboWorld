using UnityEngine;

public class GunShooting : MonoBehaviour
{
    public AudioSource fireSound; // Silah sesi kaynaðý
    public AudioClip gunShotClip; // Silah sesi klibi
    public float fireRate = 0.1f; // Ateþ etme hýzý
    private float nextFireTime = 0f; // Bir sonraki ateþ zamaný

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime) // Sol týk basýlý ve fireRate'e uygun mu?
        {
            nextFireTime = Time.time + fireRate;
            fireSound.PlayOneShot(gunShotClip); // Sesin üst üste çalmasýný saðlar
        }
    }
}
