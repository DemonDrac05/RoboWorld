using UnityEngine;

public class CyberMancubus : Boss
{
    public Transform origin;

    [Header("=== Specific Properties ===========")]
    public bool spawnRangeTrigger;

    public CyberMancubus_IdleState CyberMancubus_IdleState;
    public CyberMancubus_SpawnState CyberMancubus_SpawnState;
    public CyberMancubus_ChaseState CyberMancubus_ChaseState;
    public CyberMancubus_LeapState CyberMancubus_LeapState;

    public CyberMancubus_MeleeAttackState CyberMancubus_MeleeAttackState;
    public CyberMancubus_RangeAttackState CyberMancubus_RangeAttackState;

    public override void Awake()
    {
        stateMachine = new EnemyStateMachine();
        CyberMancubus_IdleState = new CyberMancubus_IdleState(this, stateMachine);
        CyberMancubus_SpawnState = new CyberMancubus_SpawnState(this, stateMachine);
        CyberMancubus_ChaseState = new CyberMancubus_ChaseState(this, stateMachine);
        CyberMancubus_LeapState = new CyberMancubus_LeapState(this, stateMachine);

        CyberMancubus_MeleeAttackState = new CyberMancubus_MeleeAttackState(this, stateMachine);
        CyberMancubus_RangeAttackState = new CyberMancubus_RangeAttackState(this, stateMachine);
    }

    public override void Start()
    {
        animator = GetComponent<Animator>();
        stateMachine.Initialize(CyberMancubus_LeapState);
    }
}
