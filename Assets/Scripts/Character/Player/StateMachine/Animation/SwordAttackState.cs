using UnityEngine;

public class SwordAttackState : PlayerState
{
    public new Player player;

    private readonly string swdAttack_1 = "Stable Sword Outward Slash";
    private readonly string swdAttack_2 = "Stable Sword Inward Slash";
    private readonly string swdAttack_3 = "Sword Fight One";

    private int currentAttackIndex = 0;

    private int pressCount = 0;

    private readonly string[] comboSequence;

    public SwordAttackState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        this.player = player;
        comboSequence = new string[] { swdAttack_1, swdAttack_2 };
    }

    public override void EnterState()
    {
        pressCount = 0;
        currentAttackIndex = 0;
        PlaySwordAttackAnimation(comboSequence[currentAttackIndex]);
    }

    public override void ExitState()
    {
        pressCount = 0;
        currentAttackIndex = 0;
    }

    public override void FrameUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            pressCount++;
        }

        var animStateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
        if (pressCount > 0 && animStateInfo.normalizedTime >= (40f/ 60f))
        {
            Debug.Log(pressCount);
            ProceedToNextSlash();
        }
        else if (animStateInfo.normalizedTime >= (animStateInfo.length - 0.1f))
        {
            player.stateMachine.ChangeState(player.movementState);
        }
    }

    private void ProceedToNextSlash()
    {
        if (currentAttackIndex < comboSequence.Length - 1)
        {
            currentAttackIndex++;
            PlaySwordAttackAnimation(comboSequence[currentAttackIndex]);
            
        }
        else
        {
            player.stateMachine.ChangeState(player.movementState);
        }
    }

    private void PlaySwordAttackAnimation(string animationName)
    {
        player.animator.Play(animationName);
    }

    public override void PhysicsUpdate()
    {
    }
}
