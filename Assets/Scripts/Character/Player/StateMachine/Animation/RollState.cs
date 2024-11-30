using UnityEngine;

public class RollState : PlayerState
{
    private const string FallToRoll = "Falling Forward Roll";
    private const string SprintToRoll = "Sprinting Forward Roll";

    private string nextAnimation;

    public RollState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void ExitState()
    {
        player.animator.CrossFade(nextAnimation, 0.5f);
        player.isMoving = false;
    }

    public override void FrameUpdate()
    {
        var animStateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
        nextAnimation = animStateInfo.IsName("Run With Sword") ? SprintToRoll : SprintToRoll;
        player.animator.Play(nextAnimation);

        if (animStateInfo.normalizedTime >= 1f)
        {
            player.stateMachine.ChangeState(player.movementState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
