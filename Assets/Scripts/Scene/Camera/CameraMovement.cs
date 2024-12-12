using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float MaxOfView = 60f;
    [SerializeField] private float MinOfView = 20f;
    [SerializeField] private Transform playerTransform;

    public static Camera m_Camera;
    private Vector3 offset = new Vector3(0, 10, -5); // Example offset, adjust as needed
    private const float fixedY = 13f;

    private void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_Camera.fieldOfView = (MaxOfView + MinOfView) / 2f;
    }

    private void LateUpdate()
    {
        CameraPosition();
        ZoomToView();
    }

    private void CameraPosition()
    {
        Vector3 targetPosition = playerTransform.position + offset;

        transform.position = targetPosition;
        transform.LookAt(targetPosition);
    }

    private void ZoomToView()
    {
        float scrollVal = Input.GetAxis("Mouse ScrollWheel");
        if (scrollVal != 0)
        {
            float targetFOV = Mathf.Clamp(m_Camera.fieldOfView * (1 - scrollVal), MinOfView, MaxOfView);
            m_Camera.fieldOfView = Mathf.Lerp(m_Camera.fieldOfView, targetFOV, Time.deltaTime * 10);
        }
    }
}
