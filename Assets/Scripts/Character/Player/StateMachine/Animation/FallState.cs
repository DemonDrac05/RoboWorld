
public class FallState : PlayerState
{
    private const string Falling = "Falling Idle";
    private const string Landing = "Falling To Landing";

    public FallState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void FrameUpdate()
    {
        player.animator.Play(player.isGrounded() ?  Landing : Falling);    
        if (player.animator.GetCurrentAnimatorStateInfo(0).IsName(Landing)
            && player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            player.animator.Play("Idle");
            player.stateMachine.ChangeState(player.movementState);
        }
    }
}
