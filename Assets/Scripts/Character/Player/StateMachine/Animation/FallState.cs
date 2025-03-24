
public class FallState : PlayerState
{
    private PlayerInteract playerInteract;

    private const string Falling = "Falling Idle";
    private const string Landing = "Falling To Landing";

    public FallState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        playerInteract = player.GetComponent<PlayerInteract>();
    }

    public override void EnterState()
    {
        player.Animator.Play(Falling);
    }

    public override void FrameUpdate()
    {
        if (playerInteract.isTriggerPortal)
        {
            player.Animator.SetBool("isLanding", true);
            player.stateMachine.ChangeState(player.movementState);
        }
    }
}
