using UnityEngine;
using System.Collections;

public class Turret : BaseWeapon
{
    private bool _isPlayerDetected; 

    public override void Start()
    {
        base.Start(); 
        StartCoroutine(AIDecisionLoop());
    }

    private void Update()
    {
        if (_isPlayerDetected && target != null)
        {
            RotateTowardsTarget();
        }
    }

    private IEnumerator AIDecisionLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(decisionRate);

            FindTargetInVisionCone();

            if (_isPlayerDetected && !isFiring)
            {
                StartCoroutine(StartLockOn());
            }
        }
    }

    private IEnumerator StartLockOn()
    {
        yield return new WaitForSeconds(weaponData.lockOnTime);

        if (_isPlayerDetected)
        {
            isLockedOn = true;
            StartCoroutine(FireBurstOnce());
        }
    }

    private IEnumerator FireBurstOnce()
    {
        isFiring = true;

        for (int i = 0; i < weaponData.burstCount; i++)
        {
            if (currentMagazineAmmo > 0 && isLockedOn && HasLineOfSight(target))
            {
                FireSingleShot(); 
                yield return new WaitForSeconds(weaponData.burstDelay);
            }
        }

        DeactivateTurret(); 
    }

    private void DeactivateTurret()
    {
        isFiring = false;
        isTargetInCone = false;
        isLockedOn = false;
        currentLockOnTime = 0f;
        _isPlayerDetected = false;
    }

    protected override void FindTargetInVisionCone()
    {
        target = null; 
        _isPlayerDetected = false; 

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<IDamagable>() != null)
            {
                Vector3 directionToTarget = (collider.transform.position - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

                if (angleToTarget < visionAngle / 2 && HasLineOfSight(collider.transform))
                {
                    target = collider.transform;
                    _isPlayerDetected = true;
                    break;
                }
            }
        }
    }


    public override bool HasLineOfSight(Transform potentialTarget)
    {
        Vector3 directionToTarget = (potentialTarget.position - gunBarrel.position).normalized;

        if (Physics.Raycast(gunBarrel.position, directionToTarget, out RaycastHit hit, detectionRange))
        {
            return hit.transform == potentialTarget;
        }

        return false;
    }
}