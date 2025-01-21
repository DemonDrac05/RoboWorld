using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private Drone drone;

    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        if (enemy is Drone) this.drone = (Drone)enemy;
    }

    public override void EnterState()
    {
        enemy.Animator.Play("Idle");
    }

    public override void FrameUpdate()
    {
        if (enemy.shootRangeTrigger)
        {
            enemy.stateMachine.ChangeState(enemy.attackState);
        }
        else if (enemy.chaseRangeTrigger)
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
        }
    }
}
