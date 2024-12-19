using UnityEngine;

public class CyberMancubus_IdleState : EnemyState
{
    public CyberMancubus boss;
    public CyberMancubus_IdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        this.boss = enemy as CyberMancubus;
    }

    public override void EnterState()
    {
        boss.animator.Play("Idle");
    }

    public override void FrameUpdate()
    {
        if (boss.shootRangeTrigger)
        {
            boss.stateMachine.ChangeState(boss.CyberMancubus_RangeAttackState);
        }
        else if (boss.chaseRangeTrigger)
        {
            boss.stateMachine.ChangeState(boss.CyberMancubus_ChaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
