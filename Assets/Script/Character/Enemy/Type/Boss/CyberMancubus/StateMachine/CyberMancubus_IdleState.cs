using UnityEngine;

public class CyberMancubus_IdleState : EnemyState
{
    public Enemy boss;
    public CyberMancubus_IdleState(Enemy boss, EnemyStateMachine enemyStateMachine) : base(boss, enemyStateMachine)
    {
        this.boss = boss;
    }

    public override void EnterState()
    {
        boss.animator.Play("Idle");
    }

    public override void FrameUpdate()
    {
        //if (boss.shootRangeTrigger)
        //{

        //}
        //else if (boss.chaseRangeTrigger)
        //{
        //    boss.stateMachine.ChangeState(boss.chaseState);
        //}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
