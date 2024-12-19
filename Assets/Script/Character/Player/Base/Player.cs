using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("=== Player Properties ===========")]
    public LayerMask clickableLayer;

    public float movementSpeed;
    public float rotationSpeed;

    public bool isMoving;

    [HideInInspector] public Rigidbody rigidbody;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Collider collider;

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
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();

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
