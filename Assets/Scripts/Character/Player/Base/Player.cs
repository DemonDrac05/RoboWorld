using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isVulnerable;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Collider playerCollider;

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

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider>();

        stateMachine = new PlayerStateMachine();
        movementState = new MovementState(this, stateMachine);
        swordAttackState = new SwordAttackState(this, stateMachine);
        rollState = new RollState(this, stateMachine);
        fallState = new FallState(this, stateMachine);
        healthState = new HealthState(this, stateMachine);
    }

    private void Start()
    {
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
        SetBodyOnGround();
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


    #region PHYSICS ===========
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f, this.GetComponent<PlayerMovement>().clickableLayer);
    }

    void SetLayerMaskOnDodging()
        => gameObject.layer = LayerMask.NameToLayer(isVulnerable ? "Player" : "Ignore Raycast");

    void SetBodyOnGround()
    {
        if (IsGrounded())
            player.rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }
    #endregion
}
