using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public new Enemy enemy;

    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        this.enemy = enemy;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        var lightningBeam = enemy.GetComponentInChildren<ParticleSystem>();
        if (!lightningBeam.isPlaying)
        {
            lightningBeam.Play();
        }

        CheckFacingToPlayer();

        if (!enemy.shootRangeTrigger && enemy.chaseRangeTrigger)
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
        }
    }

    void CheckFacingToPlayer()
    {
        Vector3 direction = (Player.player.transform.position - enemy.transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, enemy.rotationSpeed * Time.deltaTime);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
