using UnityEngine;

public class SwordAttackState : PlayerState
{
    private const string OutSlash = "Outward Slash";
    private const string Attacking = "isAttacking";

    public SwordAttackState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }

    public override void FrameUpdate()
    {
        player.SetAnimatorBoolOnAnimationEnd(player.movementState,OutSlash, Attacking, false);
    }
}
