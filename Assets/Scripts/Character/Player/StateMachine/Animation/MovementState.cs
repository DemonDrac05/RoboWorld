using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class MovementState : PlayerState
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

    public MovementState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        this.playerMovement = player.GetComponent<PlayerMovement>();
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
        if (player.Animator.GetCurrentAnimatorStateInfo(0).IsName(IdleState)
            || player.Animator.GetCurrentAnimatorStateInfo(0).IsName(RunState))
        {
            playerMovement.MoveCharacter();
            playerMovement.RotateCharacter();
        }
        player.Animator.SetBool(Running, player.IsMoving);
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
            yield return new WaitForSeconds(0.15F);
        }
    }
    #endregion
}
