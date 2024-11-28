using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("=== Player Properties ===========")]
    public LayerMask clickableLayer;
    public LayerMask nonClickableLayer;

    public float movementSpeed;
    public float rotationSpeed;

    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public Vector3 direction;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Collider playerCollider;

    public PlayerStateMachine stateMachine;
    public MovementState movementState;
    public SwordAttackState swordAttackState;

    [HideInInspector] public static Player player;

    private void Awake()
    {
        player = this;

        stateMachine = new PlayerStateMachine();
        movementState = new MovementState(this, stateMachine);
        swordAttackState = new SwordAttackState(this, stateMachine);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider>();

        stateMachine.Initialize(movementState);
    }

    private void FixedUpdate()
    {
        stateMachine.playerState.PhysicsUpdate();
    }
    private void Update()
    {
        stateMachine.playerState.FrameUpdate();

        RotateByCursor();
    }

    void RotateByCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 targetPoint = ray.GetPoint(rayDistance);

            Vector3 direction = targetPoint - player.transform.position;
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
