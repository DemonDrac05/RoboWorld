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
    public float rotationSpeed;

    // --- POSITION ----------
    private Vector3 charDirection;
    private Vector3 movementInput;

    // --- MOBILITY CONTROL ---
    public bool canMove { get; private set; } = false;

    // --- SINGLETON CLASS ---
    private CheckEnemyInAARange _checkEnemyInAARange;
    private Player _player;

    private void Awake()
    {
        _checkEnemyInAARange = GetComponentInChildren<CheckEnemyInAARange>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (!GamePauseManager.instance.isPaused)
        {
            HandleInput();
        }
        Debug.Log(canMove);
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
        if (!canMove || movementInput == Vector3.zero) return;

        //Vector3 targetDirection = Vector3.zero;
        //if (_checkEnemyInAARange.enemies.Count > 0)
        //{
        //    Transform closestEnemy = null;
        //    float closestDistance = Mathf.Infinity;

        //    foreach (Enemy enemy in _checkEnemyInAARange.enemies)
        //    {
        //        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        //        if (distance < closestDistance)
        //        {
        //            closestDistance = distance;
        //            closestEnemy = enemy.transform;
        //        }
        //    }

        //    if (closestEnemy != null)
        //    {
        //        targetDirection = closestEnemy.position - transform.position;
        //    }

        //    targetDirection.y = 0;

        //    if (targetDirection.sqrMagnitude > 0.01f)
        //    {
        //        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        //    }
        //}
        //else
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(charDirection);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        //}

        Quaternion targetRotation = Quaternion.LookRotation(charDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void SetMobility(bool value)
    {
        _player.SetMobility(value);
        canMove = value;
    }
}