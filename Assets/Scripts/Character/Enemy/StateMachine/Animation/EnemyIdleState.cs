using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public new Enemy enemy;

    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        this.enemy = enemy;
    }

    public override void EnterState()
    {
        enemy.animator.Play("Idle");
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
