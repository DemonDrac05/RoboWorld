using UnityEngine;

public class CyberMancubus_ChaseState : EnemyState
{
    public CyberMancubus boss;

    private float movementSpeed;
    private float rotationSpeed;

    public CyberMancubus_ChaseState(Enemy boss, EnemyStateMachine enemyStateMachine) : base(boss, enemyStateMachine)
    {
        this.movementSpeed = boss.movementSpeed;
        this.rotationSpeed = boss.rotationSpeed;
    }

    public override void EnterState()
    {
        boss.animator.Play("Walk");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        if (boss.shootRangeTrigger)
        {
            int random = Random.Range(0, 1);
            boss.stateMachine.ChangeState(random == 0 ? boss.leapState : enemy.idleState);
        }
        else
        {
            ChasePlayer();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    void ChasePlayer()
    {
        Vector3 direction = (Player.player.transform.position - enemy.transform.position).normalized;
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
}
