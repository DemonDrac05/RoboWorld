using UnityEngine;

public class CyberMancubus : Boss
{
    public CyberMancubus_IdleState idleState;
    public CyberMancubus_ChaseState chaseState;
    public CyberMancubus_LeapState leapState;

    public CyberMancubus_MeleeAttackState meleeAttackState;
    public CyberMancubus_RangeAttackState rangeAttackState;

    public override void Awake()
    {
        stateMachine = new EnemyStateMachine();
        idleState = new CyberMancubus_IdleState(this, stateMachine);
        chaseState = new CyberMancubus_ChaseState(this, stateMachine);
        leapState = new CyberMancubus_LeapState(this, stateMachine);

        meleeAttackState = new CyberMancubus_MeleeAttackState(this, stateMachine);
        rangeAttackState = new CyberMancubus_RangeAttackState(this, stateMachine);
    }

    public override void Start()
    {
        animator = GetComponent<Animator>();
        stateMachine.Initialize(idleState);
    }
}
