using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class MoveState : PlayerState
{
    private PlayerMovement playerMovement;
    private GameInputActions controls;

    // --- ANIMATIONS TRANSITIONS ----------
    private const string RollForward = "Sprinting Forward Roll";

    // --- ANIMATIONS STATES ----------
    private const string RunState = "Run With Sword";
    private const string IdleState = "Idle";

    // --- PARAMETERS TRANSITIONS ----------
    private const string Running = "isRunning";
    private const string Rolling = "isRolling";
    private const string Attacking = "isAttacking";

    // --- SHOOTING VARAIABLES ----------
    private PlayerWeapon weapon;
    private Coroutine fireCoroutine;

    // Predicate for CanMove condtion
    private Func<bool> CanMove => () => playerMovement.canMove;

    public MoveState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        this.playerMovement = player.GetComponent<PlayerMovement>();
        this.weapon = player.GetComponent<PlayerWeapon>();
    }

    public override void EnterState()
    {
        controls = new GameInputActions();

        controls.Gameplay.Enable();

        controls.Gameplay.SwordAttack.performed += context => HandleInputAction(context, OnSwordAttack);
        controls.Gameplay.Roll.performed += context => HandleInputAction(context, OnRoll);
        controls.Gameplay.BulletFire.started += context => HandleInputAction(context, OnFire);
        controls.Gameplay.BulletFire.canceled += context => HandleInputAction(context, OnFire);
    }

    public override void ExitState()
    {
        controls.Gameplay.SwordAttack.performed -= context => HandleInputAction(context, OnSwordAttack);
        controls.Gameplay.Roll.performed -= context => HandleInputAction(context, OnRoll);
        controls.Gameplay.BulletFire.started -= context => HandleInputAction(context, OnFire);
        controls.Gameplay.BulletFire.canceled -= context => HandleInputAction(context, OnFire);

        controls.Gameplay.Disable();
    }

    public override void FrameUpdate()
    {
        if (CanMove())
        {
            if (PlayerInMovementState())
            {
                playerMovement.MoveProcess();
                playerMovement.RotateCharacter();
            }
            player.Animator.SetBool(Running, player.IsMoving);
        }
        else
        {
            player.Animator.SetBool(Running, false);
        }
    }

    private bool PlayerInMovementState()
        => player.Animator.GetCurrentAnimatorStateInfo(0).IsName(IdleState)
            || player.Animator.GetCurrentAnimatorStateInfo(0).IsName(RunState);

    // --- INPUT ACTION HANDLER ----------
    private void HandleInputAction(InputAction.CallbackContext context, Action<InputAction.CallbackContext> action)
    {
        if (CanMove())
        {
            action(context);
        }
    }


    #region ANIMATION TRANSITION ==========
    /// <summary>
    /// Perform attacking by sword
    /// </summary>
    /// <param name="context"></param>
    private void OnSwordAttack(InputAction.CallbackContext context)
    {
        player.Animator.SetBool(Attacking, true);
        player.stateMachine.ChangeState(player.swordAttackState);
    }

    /// <summary>
    /// Perform rolling with stamina deduction
    /// </summary>
    /// <param name="context"></param>
    private void OnRoll(InputAction.CallbackContext context)
    {
        if (PlayerStat.playerStat.Stamina >= 50f
                && !player.Animator.GetBool(Rolling)
                && !player.Animator.GetCurrentAnimatorStateInfo(0).IsName(RollForward))
        {
            PlayerStat.playerStat.SetStamina(PlayerStat.playerStat.Stamina - 50f);

            player.Animator.SetBool(Rolling, true);
            player.stateMachine.ChangeState(player.rollState);
        }
    }

    /// <summary>
    /// Perform shooting with continuous firing
    /// </summary>
    /// <param name="context"></param>
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
            yield return new WaitForSeconds(0.15f);
        }
    }
    #endregion
}
