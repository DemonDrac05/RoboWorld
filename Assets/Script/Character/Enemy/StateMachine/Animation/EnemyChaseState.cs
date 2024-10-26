using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public new Enemy enemy;

    private readonly float movementSpeed;
    private readonly float rotationSpeed;

    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        this.enemy = enemy;

        this.movementSpeed = enemy.movementSpeed;
        this.rotationSpeed = enemy.rotationSpeed;
    }

    public override void EnterState()
    {
        enemy.animator.Play("Idle");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        if (enemy.attackRangeTrigger)
        {
            enemy.stateMachine.ChangeState(enemy.attackState);
        }
        else if (!enemy.chaseRangeTrigger)
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
        else
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (Player.player. transform.position - enemy.transform.position).normalized;
        float distance = Vector3.Distance(enemy.transform.position, Player.player.transform.position);

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (distance > 0.1f)
        {
            enemy.transform.position += movementSpeed * Time.deltaTime * direction;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
