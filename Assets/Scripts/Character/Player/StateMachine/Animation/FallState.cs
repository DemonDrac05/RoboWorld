
public class FallState : PlayerState
{
    private const string Falling = "Falling Idle";
    private const string Landing = "Falling To Landing";

    public FallState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void FrameUpdate()
    {
        if (!player.IsGrounded) 
            player.Animator.Play(Falling);
        else 
            player.Animator.SetBool("isLanding", true);

        if (player.Animator.GetCurrentAnimatorStateInfo(0).IsName(Landing))
        {
            player.Animator.Play(Landing);
            player.stateMachine.ChangeState(player.movementState);
        }
    }
}
