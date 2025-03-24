using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] private float _rotationSpeed = 10f;


    // --- POSITION ----------
    private Vector3 charDirection;
    private Vector3 movementInput;

    // --- MOBILITY CONTROL ---
    public bool canMove { get; private set; } = false;

    // --- SINGLETON CLASS ---
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (!GamePauseManager.instance.isPaused)
        {
            HandleInput();
            RotateCharacter();
        }
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        movementInput = (cameraForward * verticalInput) + (cameraRight * horizontalInput);
    }


    public void MoveProcess()
    {
        if (movementInput != Vector3.zero)
        {
            charDirection = movementInput.normalized;
            transform.position += charDirection * movementSpeed * Time.deltaTime;
            _player.SetMobility(true);
        }
        else
        {
            _player.SetMobility(false);
        }
    }
    public void RotateCharacter()
    {
        Vector3 mousePos = Input.mousePosition;

        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, clickableLayer))
        {
            Vector3 lookDir = hit.point - transform.position;
            lookDir.y = 0; 

            if (lookDir == Vector3.zero) return;
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
    }


    public void SetMobility(bool value)
    {
        _player.SetMobility(value);
        canMove = value;
    }
}