
public class FallState : PlayerState
{
    private const string Falling = "Falling Idle";
    private const string Landing = "Falling To Landing";

    public FallState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void FrameUpdate()
    {

        if (!player.IsGrounded()) player.animator.Play(Falling);
        else player.animator.SetBool("isLanding", true);

        if (player.animator.GetCurrentAnimatorStateInfo(0).IsName(Landing))
        {
            player.animator.Play(Landing);
            player.stateMachine.ChangeState(player.movementState);
        }
    }
}
