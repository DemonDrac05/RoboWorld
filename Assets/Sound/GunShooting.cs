using UnityEngine;

public class GunShooting : MonoBehaviour
{
    public AudioSource fireSound; // Silah sesi kayna��
    public AudioClip gunShotClip; // Silah sesi klibi
    public float fireRate = 0.1f; // Ate� etme h�z�
    private float nextFireTime = 0f; // Bir sonraki ate� zaman�

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime) // Sol t�k bas�l� ve fireRate'e uygun mu?
        {
            nextFireTime = Time.time + fireRate;
            fireSound.PlayOneShot(gunShotClip); // Sesin �st �ste �almas�n� sa�lar
        }
    }
}
