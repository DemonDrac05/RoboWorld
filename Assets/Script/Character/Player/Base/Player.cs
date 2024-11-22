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
    }
}
