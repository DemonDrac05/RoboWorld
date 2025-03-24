using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("=== Enemy Properties ===========")]
    public float movementSpeed;
    public float rotationSpeed;

    [HideInInspector] public Animator Animator;
    [HideInInspector] public Collider Collider;
    [HideInInspector] public Rigidbody Rigidbody;

    [HideInInspector] public bool chaseRangeTrigger;
    [HideInInspector] public bool shootRangeTrigger;

    public EnemyStateMachine stateMachine;
    public EnemyIdleState idleState;
    public EnemyChaseState chaseState;
    public EnemyAttackState attackState;

    public static Enemy enemy;
    protected Player _player;

    public virtual void Awake()
    {
        enemy = this;

        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine);
        chaseState = new EnemyChaseState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
    }

    public virtual void OnEnable()
    {
        Animator = GetComponentInChildren<Animator>();
        Collider = GetComponent<Collider>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public virtual void Start()
    {
        stateMachine.Initialize(idleState);
        _player = Player.player;
    }

    public virtual void FixedUpdate()
    {
        stateMachine.enemyState.PhysicsUpdate();
    }
    public virtual void Update()
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
