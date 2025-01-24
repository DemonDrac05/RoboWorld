using UnityEngine;
using System.Collections.Generic;

public class Drone : Enemy
{
    private const string ActivateTrigger = "Activate";
    private const string IsChasingBool = "isChasing";
    private const string IsFiringBool = "isFiring";

    [Header("=== Follow Settings ===")]
    [SerializeField] private float _attackDistance = 5f; // Distance to maintain from the player
    [SerializeField] private float _followSpeed = 5f;
    [SerializeField] private float _stoppingDistance = 1f;

    [Header("=== Shooting Settings ===")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private List<Transform> firePoints = new List<Transform>();
    [SerializeField] private float bulletSpeed = 20f;

    private float _fireTimer = 0f;
    private float _cooldownTimer = 0f;
    private const float CooldownDuration = 2f;
    private const float FireDuration = 2f;
    private int _currentFirePointIndex = 0;

    public override void Update()
    {
        UpdateAnimationState();
        MoveToAttackPoint();
        RotateTowardsPlayer();
        SetPosition();
    }

    private void SetPosition()
    {
        transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
    }

    private void UpdateAnimationState()
    {
        bool shouldActivate = chaseRangeTrigger || shootRangeTrigger;
        bool isFiring = shootRangeTrigger && _cooldownTimer <= 0f;
        bool isChasing = chaseRangeTrigger;

        if (shouldActivate)
        {
            Animator.SetTrigger(ActivateTrigger);
        }

        Animator.SetBool(IsFiringBool, isFiring);
        Animator.SetBool(IsChasingBool, isChasing && !isFiring);

        UpdateTimers(isFiring);

        if (isFiring)
        {
            Shoot();
        }
    }

    private void UpdateTimers(bool isFiring)
    {
        if (isFiring)
        {
            _fireTimer += Time.deltaTime;
            if (_fireTimer >= FireDuration)
            {
                _fireTimer = 0f;
                _cooldownTimer = CooldownDuration;
            }
        }
        else if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0f)
            {
                _cooldownTimer = 0;
            }
        }
    }

    private void Shoot()
    {
        for (int i = 0; i < firePoints.Count; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoints[i].position, firePoints[i].rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                bulletRb.linearVelocity = firePoints[i].forward * bulletSpeed;
            } 
        }

        _currentFirePointIndex = (_currentFirePointIndex + 1) % firePoints.Count;
    }

    private void MoveToAttackPoint()
    {
        if (_player == null || !chaseRangeTrigger) return;

        Vector3 directionToPlayer = _player.transform.position - transform.position;

        Vector3 directionToPlayerXZ = new Vector3(directionToPlayer.x, 0, directionToPlayer.z).normalized;

        Vector3 targetPosition = _player.transform.position - (directionToPlayerXZ * _attackDistance);

        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > _stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * movementSpeed);
        }
    }

    private void RotateTowardsPlayer()
    {
        if (_player == null) return;

        Vector3 directionToPlayer = _player.transform.position - transform.position;
        Vector3 directionToPlayerXZ = new Vector3(directionToPlayer.x, 0, directionToPlayer.z).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayerXZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}