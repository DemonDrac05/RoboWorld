using UnityEngine;

public class CyberMancubus_LeapState : EnemyState
{
    private CyberMancubus boss;

    private float distance = 4.408319f;

    private Transform origin;

    public CyberMancubus_LeapState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        this.boss = enemy as CyberMancubus;
        this.origin = boss.origin;
    }

    public override void EnterState()
    {
        boss.FacingToPlayer();
    }

    public override void ExitState()
    {
    }

    public override void FrameUpdate()
    {
        boss.animator.Play("LedgeUp");

        if (boss.animator.GetCurrentAnimatorStateInfo(0).IsName("LedgeUp") &&
            boss.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            this.origin = boss.origin;
            boss.stateMachine.ChangeState(boss.CyberMancubus_IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
