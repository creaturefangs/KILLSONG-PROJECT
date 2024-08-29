using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BaseWeapon : MonoBehaviour
{
    [Header("Weapon Configuration")]
    public SO_Weapon weaponData;

    public int currentMagazineAmmo;
    public int currentReserveAmmo;
    public bool isReloading = false;
    public bool isFiring = false;

    [Header("AI Settings")]
    [Tooltip("Detection range for the AI to start shooting")]
    public float detectionRange = 20f;

    [Tooltip("Rate at which the AI checks to shoot")]
    public float decisionRate = 0.5f;

    [Header("Vision Cone Settings")]
    [Tooltip("Angle of the vision cone in degrees")]
    public float visionAngle = 45.0f; 

    public Transform gunBarrel; 

    public Transform target;
    public bool isTargetInCone;
    public bool isLockedOn;
    public float currentLockOnTime = 0f;

    [Header("Rotation Settings")]
    [Tooltip("Should the weapon rotate on all axes or only Y-axis")]
    public bool rotateOnYAxisOnly = true; 

    private AudioSource _audioSource; 

    public virtual void Start()
    {
        if (weaponData == null)
        {
            Debug.LogError("Weapon data not assigned to " + gameObject.name);
            return;
        }

        _audioSource = GetComponent<AudioSource>(); 
        currentMagazineAmmo = weaponData.startingMagazineAmmo;
        currentReserveAmmo = weaponData.maxReserveAmmo;

        StartCoroutine(AIDecisionLoop());
    }

    private IEnumerator AIDecisionLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(decisionRate);

            FindTargetInVisionCone();

            if (target != null)
            {
                isTargetInCone = IsTargetInVisionCone();

                if (isTargetInCone)
                {
                    RotateTowardsTarget();

                    if (!isLockedOn)
                    {
                        currentLockOnTime += decisionRate;

                        if (currentLockOnTime >= weaponData.lockOnTime)
                        {
                            isLockedOn = true;
                            Fire();
                        }
                    }
                }
                else
                {
                    ResetLockOn();
                }
            }
            else
            {
                ResetLockOn();
            }
        }
    }

    private void ResetLockOn()
    {
        currentLockOnTime = 0f;
        isLockedOn = false;
    }

    public void RotateTowardsTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        if (rotateOnYAxisOnly)
        {
            direction.y = 0; 
        }

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * weaponData.rotationSpeed);
    }

    protected virtual void FindTargetInVisionCone()
    {
        target = null; 
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
                    break;
                }
            }
        }
    }

    public bool IsTargetInVisionCone()
    {
        if (target == null) return false;

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        return angleToTarget < visionAngle / 2 && Vector3.Distance(transform.position, target.position) <= detectionRange && HasLineOfSight(target);
    }

    public virtual bool HasLineOfSight(Transform potentialTarget)
    {
        Vector3 directionToTarget = (potentialTarget.position - gunBarrel.position).normalized;

        if (Physics.Raycast(gunBarrel.position, directionToTarget, out RaycastHit hit, detectionRange))
        {
            return hit.transform == potentialTarget;
        }

        return false;
    }

    public virtual void Fire()
    {
        if (isReloading || isFiring || !isLockedOn)
            return;

        if (currentMagazineAmmo <= 0)
        {
            StartCoroutine(Reload()); 
            return;
        }

        switch (weaponData.weaponType)
        {
            case WeaponType.OneShot:
            case WeaponType.SingleFire:
                FireSingleShot();
                break;
            case WeaponType.BurstFire:
                StartCoroutine(FireBurst());
                break;
            case WeaponType.Automatic:
                StartCoroutine(FireAutomatic());
                break;
        }
    }


    protected virtual void FireSingleShot()
    {
        if (currentMagazineAmmo > 0)
        {
            FireProjectile();

            if (weaponData.muzzleFlashPrefab != null)
            {
                GameObject muzzleFlash = Instantiate(weaponData.muzzleFlashPrefab, gunBarrel.position, gunBarrel.rotation);
                Destroy(muzzleFlash, 0.5f); 
            }

            PlayFireSound();
            currentMagazineAmmo--;

            if (currentMagazineAmmo <= 0)
            {
                StartCoroutine(Reload());
            }
        }
    }



    private void FireProjectile()
    {
        GameObject newBullet = Instantiate(weaponData.bullet.bulletPrefab, gunBarrel.position, gunBarrel.rotation);
        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        rb.velocity = (target.position - gunBarrel.position).normalized * weaponData.bullet.bulletSpeed;
    }

    protected virtual IEnumerator FireBurst()
    {
        isFiring = true;
        for (int i = 0; i < weaponData.burstCount; i++)
        {
            if (currentMagazineAmmo <= 0)
            {
                StartCoroutine(Reload());
                break;
            }

            FireSingleShot();
            yield return new WaitForSeconds(weaponData.burstDelay);
        }
        isFiring = false;
    }

    protected virtual IEnumerator FireAutomatic()
    {
        isFiring = true;
        while (currentMagazineAmmo > 0)
        {
            FireSingleShot();
            if (currentMagazineAmmo <= 0)
            {
                StartCoroutine(Reload());
                break;
            }
            yield return new WaitForSeconds(weaponData.fireRate);
        }
        isFiring = false;
    }


    private void PlayFireSound()
    {
        if (_audioSource != null && weaponData.fireSound != null)
        {
            _audioSource.PlayOneShot(weaponData.fireSound);
        }
    }

    public virtual IEnumerator Reload()
    {
        if (isReloading) yield break;

        isReloading = true;

        yield return new WaitForSeconds(weaponData.reloadTime);

        int ammoNeeded = weaponData.maxMagazineAmmo - currentMagazineAmmo;
        int ammoToLoad = Mathf.Min(ammoNeeded, currentReserveAmmo);
        currentMagazineAmmo += ammoToLoad;
        currentReserveAmmo -= ammoToLoad;

        isReloading = false;
    }

    public void ResetAmmo()
    {
        currentMagazineAmmo = weaponData.startingMagazineAmmo;
        currentReserveAmmo = weaponData.maxReserveAmmo;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2, 0) * transform.forward * detectionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2, 0) * transform.forward * detectionRange;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);
    }
}