using UnityEngine;

public class Drone : Enemy
{
    private const string ActivateTrigger = "Activate";
    private const string IsChasingBool = "isChasing";
    private const string IsFiringBool = "isFiring";

    private float _fireTimer = 0f;
    private float _cooldownTimer = 0f;
    private const float CooldownDuration = 2f; 
    private const float FireDuration = 2f; 

    public override void Update()
    {
        UpdateAnimationState();
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
}