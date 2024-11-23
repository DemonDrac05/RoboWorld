using UnityEngine;

public class MovementState : PlayerState
{
    public new Player player;

    private Vector3 targetPosition;

    private readonly float movementSpeed;
    private readonly float rotationSpeed;

    public MovementState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        this.player = player;

        this.movementSpeed = player.movementSpeed;
        this.rotationSpeed = player.rotationSpeed;
    }

    public override void EnterState()
    {
        player.animator.Play("Idle");
    }

    public override void FrameUpdate()
    {
        MoveByCursor();

        StateTransition();
    }

    void MoveByCursor()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = CameraMovement.m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, player.clickableLayer))
            {
                targetPosition = hit.point;
                player.isMoving = true;
            }
            //else if (Physics.Raycast(ray, out hit, Mathf.Infinity, player.nonClickableLayer))
            //{
            //    targetPosition = player.transform.position;
            //    player.isMoving = false;
            //}
        }
        if (player.isMoving)
        {
            MovePlayer();
        }
        else
        {
            targetPosition = player.transform.position;
            player.animator.Play("Idle");
        }
    }

    void MovePlayer()
    {
        float distance = Vector3.Distance(player.transform.position, targetPosition);
        if (distance > 0.1f)
        {
            player.transform.position += player.direction * movementSpeed * Time.deltaTime;
            player.animator.Play("Run With Sword");
        }
        else
        {
            player.isMoving = false;
            player.animator.Play("Idle");
        }
    }

    void StateTransition()
    {
        // -- Sword Attack State ----------
        if (Input.GetMouseButton(1))
        {
            player.stateMachine.ChangeState(player.swordAttackState);
        }

        // -- Gun Shoot State ----------
        if (Input.GetKeyDown(KeyCode.Z))
        {

        }
    }
}
