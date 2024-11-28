using System.Collections.Generic;
using UnityEngine;

public class CyberMancubus_RangeAttackState : EnemyState
{
    private CyberMancubus boss;

    private int rand;

    private const string ShootAnimation00 = "Shoot_Normal";
    private const string ShootAnimation01 = "Shoot_Mortar";
    private const string ShootAnimation02 = "Shoot_Rapid";

    private List<string> ShootAnimations = new List<string>();

    public CyberMancubus_RangeAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        this.boss = enemy as CyberMancubus;
    }

    public override void EnterState()
    {
        ShootAnimations.Add(ShootAnimation00);
        ShootAnimations.Add(ShootAnimation01);
        ShootAnimations.Add(ShootAnimation02);

        rand = Random.Range(0, ShootAnimations.Count);
    }

    public override void ExitState()
    {
        ShootAnimations.Clear();
    }

    public override void FrameUpdate()
    {
        var animator = boss.animator.GetCurrentAnimatorStateInfo(0);

        if (ShootAnimations.Count > 0)
        {
            boss.animator.Play(ShootAnimations[rand]);

            if (animator.IsName(ShootAnimations[rand]) && animator.normalizedTime >= 1f)
            {
                ShootAnimations.RemoveAt(rand);

                if (boss.shootRangeTrigger && ShootAnimations.Count > 0)
                {
                    rand = Random.Range(0, ShootAnimations.Count);
                    boss.animator.Play(ShootAnimations[rand]);
                }
                else if (!boss.shootRangeTrigger || ShootAnimations.Count == 0)
                {
                    boss.stateMachine.ChangeState(boss.CyberMancubus_LeapState);
                }
            }
        }
        else
        {
            boss.stateMachine.ChangeState(boss.CyberMancubus_LeapState);
        }
    }

    public override void PhysicsUpdate()
    {
        boss.FacingToPlayer();
    }
}
