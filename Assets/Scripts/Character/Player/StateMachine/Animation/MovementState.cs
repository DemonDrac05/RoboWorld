using UnityEngine;

public class MovementState : PlayerState
{
    public new Player player;

    private Vector3 targetPosition;

    private readonly float movementSpeed;
    private readonly float rotationSpeed;

    private const string Idle = "Idle";
    private const string RunWithSword = "Run With Sword";

    public MovementState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        this.player = player;

        this.movementSpeed = player.movementSpeed;
        this.rotationSpeed = player.rotationSpeed;
    }

    public override void EnterState()
    {
        player.animator.Play(Idle);
    }

    public override void FrameUpdate()
    {
        MoveByCursor();
        RotateByCursor();
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
            player.animator.Play(Idle);
        }
    }

    void RotateByCursor()
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

    void MovePlayer()
    {
        player.direction = (targetPosition - player.transform.position).normalized;
        float distance = Vector3.Distance(player.transform.position, targetPosition);
        if (distance > 0.1f)
        {
            player.transform.position += player.direction * movementSpeed * Time.deltaTime;
            player.animator.Play(RunWithSword);
        }
        else
        {
            player.isMoving = false;
            player.animator.Play(Idle);
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

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    player.stateMachine.ChangeState(player.rollState);
        //}
    }
}
