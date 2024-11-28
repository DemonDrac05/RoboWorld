using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] private float MaxOfView;
    [SerializeField] private float MinOfView;
    [SerializeField] private Transform playerTransform;

    public static Camera m_Camera;

    private const float AngleOfView = 60f;

    private void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_Camera.fieldOfView = (MaxOfView + MinOfView) / 2f;
    }

    private void Update()
    {
        CameraPosition();
        ZoomToView();
    }

    private void CameraPosition()
    {
        float radianOfView = Mathf.Deg2Rad * AngleOfView;
        float heightOfView = this.transform.position.y;

        float distanceCam2Player = heightOfView * Mathf.Tan(radianOfView / 3);
        float horizontalOnPlane = playerTransform.position.z - distanceCam2Player;

        float verticalOnPlane = playerTransform.position.x;
        Vector3 cameraPosition = new(verticalOnPlane, heightOfView, horizontalOnPlane);
        this.transform.position = cameraPosition;
    }

    private void ZoomToView()
    {
        ScrollToView();
        TouchToView();
    }

    private void ScrollToView()
    {
        float scrollVal = Input.GetAxis("Mouse ScrollWheel");
        if (scrollVal != 0)
        {
            m_Camera.fieldOfView = Mathf.Clamp(m_Camera.fieldOfView * (1 - scrollVal), MinOfView, MaxOfView);
        }
    }
    
    private void TouchToView()
    {
        int touchVal = Input.touchCount;
        if (touchVal == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentTouchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float pinchDelta = prevTouchDeltaMag - currentTouchDeltaMag;

            m_Camera.fieldOfView = Mathf.Clamp(m_Camera.fieldOfView + pinchDelta, MinOfView, MaxOfView);
        }
    }
}
