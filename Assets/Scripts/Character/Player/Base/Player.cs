using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("=== Player Properties ===========")]
    public LayerMask clickableLayer;
    public LayerMask nonClickableLayer;

    public float movementSpeed;
    public float rotationSpeed;

    [HideInInspector] public bool isMoving = false;
    //public bool isGrounded = false;
    public float groundCheckDistance;
    private float bufferCheckDistance = 0.1f;
    [HideInInspector] public bool isVunerable = true;
    [HideInInspector] public Vector3 direction;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Collider playerCollider;

    // --- UNITY BUILT-IN ----------

    // --- STATE MACHINE ----------
    public PlayerStateMachine stateMachine;
    public MovementState movementState;
    public SwordAttackState swordAttackState;
    public RollState rollState;
    public FallState fallState;
    public HealthState healthState;

    // --- SINGLETON INSTANCE -----------
    [HideInInspector] public static Player player;

    private void Awake()
    {
        player = this;

        stateMachine = new PlayerStateMachine();
        movementState = new MovementState(this, stateMachine);
        swordAttackState = new SwordAttackState(this, stateMachine);
        rollState = new RollState(this, stateMachine);
        fallState = new FallState(this, stateMachine);
        healthState = new HealthState(this, stateMachine);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        playerCollider = GetComponent<Collider>();

        stateMachine.Initialize(fallState);
        PlayerStat.playerStat.GetCheckpoint(transform);
    }

    private void FixedUpdate()
    {
        stateMachine.playerState.PhysicsUpdate();

        SetLayerMaskOnDodging();
    }

    private void Update()
    {
        stateMachine.playerState.FrameUpdate();

        //groundCheckDistance = (GetComponent<CapsuleCollider>().height / 2) + bufferCheckDistance;
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, -transform.up, out hit, groundCheckDistance))
        //{
        //    isGrounded = true;
        //}
        //else
        //{
        //    isGrounded = false;
        //}
        //Debug.Log(isGrounded);

        if (isGrounded())
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    public void SetAnimatorBoolOnAnimationEnd(PlayerState nextState, string animationStateName, string boolParameter, bool targetState)
    {
        AnimatorStateInfo currentState = player.animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsName(animationStateName) && currentState.normalizedTime >= 1.0f)
        {
            player.animator.SetBool(boolParameter, targetState);
            if (nextState != null) stateMachine.ChangeState(nextState);
        }
    }

    public bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f, clickableLayer);
    }


    void SetLayerMaskOnDodging() 
        => player.gameObject.layer = LayerMask.NameToLayer(isVunerable ? "Player" : "Ignore Raycast");
}
