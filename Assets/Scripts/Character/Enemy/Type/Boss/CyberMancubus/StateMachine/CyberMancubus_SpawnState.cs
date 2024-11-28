using UnityEngine;

public class CyberMancubus_SpawnState : EnemyState
{
    public CyberMancubus boss;

    private const string Spawn00 = "Spawn_00";
    private const string Spawn01 = "Spawn_01";

    public CyberMancubus_SpawnState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        this.boss = enemy as CyberMancubus;
    }

    public override void FrameUpdate()
    {
        var animator = boss.animator.GetCurrentAnimatorStateInfo(0);
        if (boss.spawnRangeTrigger)
        {
            boss.animator.Play(Spawn00);
        }
        else
        {
            if (animator.IsName(Spawn00))
            {
                boss.animator.Play(Spawn01);
            }
        }

        if (animator.IsName(Spawn00) && animator.normalizedTime >= 1f)
        {
            boss.spawnRangeTrigger = false;
        }
        else if (animator.IsName(Spawn01) && animator.normalizedTime >= 1f)
        {
            boss.stateMachine.ChangeState(boss.CyberMancubus_IdleState);
        }
    }
}
