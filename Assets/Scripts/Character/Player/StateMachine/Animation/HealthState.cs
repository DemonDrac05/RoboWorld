using System;
using UnityEngine.SceneManagement;

public class HealthState : PlayerState
{
    private const string BackwardKO = "Knocked Out Backward";
    private const string ForwardKO  = "Knocked Out Forward";
    private const string Staggered  = "Get Damage";

    public HealthState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {}

    public override void EnterState()
        => player.Animator.Play(PlayerStat.playerStat.Health > 0f 
                                ? Staggered : GetRandomRange(1, 2) == 1 ? BackwardKO : ForwardKO);

    public override void ExitState() => player.SetVulnerability(true);
    public override void PhysicsUpdate() => player.SetVulnerability(false);

    public override void FrameUpdate()
    {
        if (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            if (player.Animator.GetCurrentAnimatorStateInfo(0).IsName(Staggered))
            {
                player.Animator.Play("Idle");
                player.stateMachine.ChangeState(player.movementState);
            }
            else if (player.Animator.GetCurrentAnimatorStateInfo(0).IsName(BackwardKO)
                    || player.Animator.GetCurrentAnimatorStateInfo(0).IsName(ForwardKO))
            {
                SceneManager.LoadScene("MenuScene");
            }
        }
    }

    public static int GetRandomRange(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max + 1);
    }
}
