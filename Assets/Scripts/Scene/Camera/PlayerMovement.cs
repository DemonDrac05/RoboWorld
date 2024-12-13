using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("=== Camera Movement ==========")]
    public Camera mainCamera;

    [Header("=== Layer Terrain ==========")]
    public LayerMask clickableLayer;
    public LayerMask nonClickableLayer;

    [Header("=== Move Properties ===========")]
    public float movementSpeed;
    public float rotationSpeed;

    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public bool isVunerable = true;

    private Vector3 targetPosition;
    private Vector3 charDirection;

    protected Player player;

    private void Awake()
    {
        player = GetComponent<Player>();    
    }

    public void MoveCharacter()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayer))
            {
                targetPosition = hit.point;
                isMoving = true;
            }
        }

        if (isMoving)
        {
            MoveProcess();
        }
        else
        {
            targetPosition = transform.position;
            player.animator.SetBool("isRunning", false);
        }
    }

    public void MoveProcess()
    {
        charDirection = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance > 0.1f)
        {
            transform.position += charDirection * movementSpeed * Time.deltaTime;
            player.animator.SetBool("isRunning", true);
        }
        else
        {
            isMoving = false;
            player.animator.SetBool("isRunning", false);
        }
    }

    public void RotateCharacter()
    {
        Vector3 mousePosition = Input.mousePosition;

        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;

            Vector3 direction = targetPosition - transform.position;
            direction.y = 0; 

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }
}
