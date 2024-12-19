using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MovementState : PlayerState
{
    public new Player player;

    private Vector3 targetPosition;

    private readonly float movementSpeed;
    private readonly float rotationSpeed;

    private bool isMoving;

    public MovementState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        this.player = player;

        this.movementSpeed = player.movementSpeed;
        this.rotationSpeed = player.rotationSpeed;

        this.isMoving = player.isMoving;
    }

    public override void EnterState()
    {
        isMoving = player.isMoving = false;
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
                isMoving = true;
            }
        }
        if (isMoving)
        {
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        Vector3 direction = (targetPosition - player.transform.position).normalized;
        float distance = Vector3.Distance(player.transform.position, targetPosition);

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (distance > 0.1f)
        {
            player.transform.position += direction * movementSpeed * Time.deltaTime;
            player.animator.Play("Run With Sword");
        }
        else
        {
            isMoving = false;
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
