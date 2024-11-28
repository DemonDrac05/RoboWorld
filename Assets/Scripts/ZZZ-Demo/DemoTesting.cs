using UnityEngine;

public class DemoTesting : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        // Make the Canvas face the camera
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0f, 0f);
    }
}
