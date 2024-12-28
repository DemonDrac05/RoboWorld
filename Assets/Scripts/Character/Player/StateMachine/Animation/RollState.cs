using UnityEngine;

public class RollState : PlayerState
{
    private const string RollForward = "Sprinting Forward Roll";
    private const string Rolling = "isRolling";

    public RollState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState() => player.SetVulnerability(false);

    public override void ExitState() => player.SetVulnerability(true);

    public override void FrameUpdate() => player.SetAnimatorBoolOnAnimationEnd(player.movementState, RollForward, Rolling, false);
}
