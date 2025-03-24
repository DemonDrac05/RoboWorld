using UnityEngine;

public class Player : MonoBehaviour, IComponents, IVariables
{
    // --- IVariables ----------
    public bool IsVulnerable { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsMoving { get; private set; }

    // --- ICOMPONENTS -----------
    public Rigidbody Rigidbody { get; private set; }
    public Collider Collider { get; private set; }
    public Animator Animator { get; private set; }

    // --- STATE MACHINE ----------
    public PlayerStateMachine stateMachine { get; private set; }
    public MoveState movementState;
    public RollState rollState;
    public FallState fallState;

    public SwordAttackState swordAttackState;
    public HealthState healthState;

    // --- SINGLETON INSTANCE ----------
    public static Player player { get; private set; }

    private void Awake()
    {
        player = this;

        InitializeComponents();
        InitalizeStateMachine();
    }
    private void InitializeComponents()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        Collider = GetComponent<Collider>();
    }

    private void InitalizeStateMachine()
    {
        stateMachine = new PlayerStateMachine();
        var factory = new PlayerStateFactory(this, stateMachine);

        movementState = (MoveState)factory.CreateState<MoveState>();
        rollState = (RollState)factory.CreateState<RollState>();
        fallState = (FallState)factory.CreateState<FallState>();

        swordAttackState = (SwordAttackState)factory.CreateState<SwordAttackState>();
        healthState = (HealthState)factory.CreateState<HealthState>();
    }

    private void Start()
    {
        // --- SET 1. STATE ----------
        stateMachine.Initialize(fallState);
    }


    private void FixedUpdate()
    {
        stateMachine.playerState.PhysicsUpdate();
    }

    private void Update()
    {
        stateMachine.playerState.FrameUpdate();
    }

    public void SetAnimatorBoolOnAnimationEnd(PlayerState nextState, string animationStateName, string boolParameter, bool targetState)
    {
        AnimatorStateInfo currentState = player.Animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsName(animationStateName) && currentState.normalizedTime >= 1.0f)
        {
            player.Animator.SetBool(boolParameter, targetState);
        }
        else if (player.Animator.GetBool(boolParameter) == targetState)
        {
            if (nextState != null) stateMachine.ChangeState(nextState);
        }
    }

    public void SetMobility(bool moveable) => IsMoving = moveable;
    public void SetGrounded(bool grounded) => IsGrounded = grounded;
    public void SetVulnerability(bool vulnerable) => IsVulnerable = vulnerable;
}
