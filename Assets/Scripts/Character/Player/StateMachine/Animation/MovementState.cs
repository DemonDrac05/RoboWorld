using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class MovementState : PlayerState
{
    private GameInputActions controls;
    private Vector3 targetPosition;

    private readonly float movementSpeed;
    private readonly float rotationSpeed;

    // --- ANIMATIONS TRANSITIONS ----------
    private const string RollForward = "Sprinting Forward Roll";

    // --- PARAMETERS TRANSITIONS ----------
    private const string Running = "isRunning";
    private const string Rolling = "isRolling";
    private const string Attacking = "isAttacking";

    // --- SHOOTING VARAIABLES ----------
    private PlayerWeapon weapon;
    private Coroutine fireCoroutine;

    public MovementState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        this.movementSpeed = player.movementSpeed;
        this.rotationSpeed = player.rotationSpeed;

        this.weapon = player.GetComponent<PlayerWeapon>();
    }

    public override void EnterState()
    {
        controls = new GameInputActions();

        controls.Gameplay.Enable();

        controls.Gameplay.SwordAttack.performed += OnSwordAttack;
        controls.Gameplay.Roll.performed += OnRoll;

        controls.Gameplay.BulletFire.started += OnFire;
        controls.Gameplay.BulletFire.canceled += OnFire;
    }

    public override void ExitState()
    {
        controls.Gameplay.SwordAttack.performed -= OnSwordAttack;
        controls.Gameplay.Roll.performed -= OnRoll;

        controls.Gameplay.BulletFire.performed -= OnFire;
        controls.Gameplay.BulletFire.canceled -= OnFire;

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
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, player.clickableLayer))
            {
                // Only update targetPosition if it's a new point to avoid unnecessary updates
                if (Vector3.Distance(targetPosition, hit.point) > 0.1f)
                {
                    targetPosition = hit.point;
                    player.isMoving = true;
                }
            }
        }

        if (player.isMoving)
        {
            MovePlayer();
        }
        else
        {
            // Ensure the animation is updated only when necessary
            if (player.animator.GetBool(Running))
            {
                player.animator.SetBool(Running, false);
            }
        }
    }

    private void MovePlayer()
    {
        Vector3 targetDirection = (targetPosition - player.transform.position).normalized;

        // Smooth movement with damping
        player.direction = Vector3.Lerp(player.direction, targetDirection, Time.deltaTime * rotationSpeed);

        float distance = Vector3.Distance(player.transform.position, targetPosition);
        if (distance > 0.1f)
        {
            player.transform.position += player.direction * movementSpeed * Time.deltaTime;

            // Smooth transition to enable animation state
            if (!player.animator.GetBool(Running))
            {
                player.animator.SetBool(Running, true);
            }
        }
        else
        {
            // Stop moving when close enough
            player.isMoving = false;

            // Smoothly update animation state
            if (player.animator.GetBool(Running))
            {
                player.animator.SetBool(Running, false);
            }
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

    private void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            fireCoroutine = player.StartCoroutine(FireContinuously());
        }
        else if (context.canceled)
        {
            if (fireCoroutine != null)
            {
                player.StopCoroutine(fireCoroutine);
                fireCoroutine = null;
            }
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            WeaponManager.instance.FireBullet(weapon.bulletPrefab, weapon.firePoint, weapon.bulletSpeed);
            yield return new WaitForSeconds(0.15F);
        }
    }
}
