using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("=== Enemy Properties ===========")]
    public float movementSpeed;
    public float rotationSpeed;

    [HideInInspector] public Animator animator;
    [HideInInspector] public Collider collider;

    [HideInInspector] public bool chaseRangeTrigger;
    [HideInInspector] public bool shootRangeTrigger;

    public EnemyStateMachine stateMachine;
    public EnemyIdleState idleState;
    public EnemyChaseState chaseState;
    public EnemyAttackState attackState;

    public static Enemy enemy;
    public virtual void Awake()
    {
        enemy = this;

        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine);
        chaseState = new EnemyChaseState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
    }

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();

        stateMachine.Initialize(idleState);
    }

    private void FixedUpdate()
    {
        stateMachine.enemyState.PhysicsUpdate();
    }
    private void Update()
    {
        stateMachine.enemyState.FrameUpdate();
    }

    public void FacingToPlayer()
    {
        Vector3 direction = (Player.player.transform.position - this.transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
