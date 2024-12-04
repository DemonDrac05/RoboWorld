using UnityEngine;
using UnityEngine.InputSystem;

public class MovementState : PlayerState
{
    private GameInputActions controls;
    private Vector3 targetPosition;

    private readonly float movementSpeed;
    private readonly float rotationSpeed;

    private const string Idle = "Idle";
    private const string RunWithSword = "Run With Sword";
    private const string RollForward = "Sprinting Forward Roll";

    private const string Running = "isRunning";
    private const string Rolling = "isRolling";
    private const string Attacking = "isAttacking";

    public MovementState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        this.movementSpeed = player.movementSpeed;
        this.rotationSpeed = player.rotationSpeed;
    }

    public override void EnterState()
    {
        controls = new GameInputActions();

        controls.Gameplay.Enable();

        controls.Gameplay.SwordAttack.performed += OnSwordAttack;
        controls.Gameplay.Roll.performed += OnRoll;
    }

    public override void ExitState()
    {
        controls.Gameplay.SwordAttack.performed -= OnSwordAttack;
        controls.Gameplay.Roll.performed -= OnRoll;

        controls.Gameplay.Disable();
    }

    public override void FrameUpdate()
    {
        MoveByCursor();
        RotateByCursor();
    }

    private void MoveByCursor()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Ray ray = CameraMovement.m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, player.clickableLayer))
            {
                targetPosition = hit.point;
                player.isMoving = true;
            }
        }

        if (player.isMoving)
        {
            MovePlayer();
        }
        else
        {
            targetPosition = player.transform.position;

            player.animator.SetBool(Running, false);
        }
    }

    private void MovePlayer()
    {
        player.direction = (targetPosition - player.transform.position).normalized;
        float distance = Vector3.Distance(player.transform.position, targetPosition);
        if (distance > 0.1f)
        {
            player.transform.position += player.direction * movementSpeed * Time.deltaTime;
            player.animator.SetBool(Running, true);
        }
        else
        {
            player.isMoving = false;
            player.animator.SetBool(Running, false);
        }
    }

    private void RotateByCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 targetPoint = ray.GetPoint(rayDistance);

            Vector3 direction = targetPoint - player.transform.position;
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnSwordAttack(InputAction.CallbackContext context)
    {
        player.animator.SetBool(Attacking, true);
        player.stateMachine.ChangeState(player.swordAttackState);
    }

    private void OnRoll(InputAction.CallbackContext context)
    {
        if (PlayerStat.playerStat.Stamina >= 50f
                && !player.animator.GetBool(Rolling)
                && !player.animator.GetCurrentAnimatorStateInfo(0).IsName(RollForward))
        {
            PlayerStat.playerStat.SetStamina(PlayerStat.playerStat.Stamina - 50f);

            player.animator.SetBool(Rolling, true);
            player.stateMachine.ChangeState(player.rollState);
        }
    }
}
