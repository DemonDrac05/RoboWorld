using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("=== Player Properties ===========")]
    public LayerMask clickableLayer;
    public LayerMask nonClickableLayer;

    public float movementSpeed;
    public float rotationSpeed;

    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public bool isVunerable = true;
    [HideInInspector] public Vector3 direction;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Collider playerCollider;

    public PlayerStateMachine stateMachine;
    public MovementState movementState;
    public SwordAttackState swordAttackState;
    public RollState rollState;

    [HideInInspector] public static Player player;

    private void Awake()
    {
        player = this;

        stateMachine = new PlayerStateMachine();
        movementState = new MovementState(this, stateMachine);
        swordAttackState = new SwordAttackState(this, stateMachine);
        rollState = new RollState(this, stateMachine);
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

        SetLayerMaskOnDodging();

    }
    private void Update()
    {
        stateMachine.playerState.FrameUpdate();
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

    void SetLayerMaskOnDodging() 
        => player.gameObject.layer = LayerMask.NameToLayer(isVunerable ? "Player" : "Ignore Raycast");
}
