using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
    [Header("Turret Settings")]
    public Transform target;
    //range that the turret can see
    public float visionRange = 20.0f; 
    //speed at which the turret rotates towards the target
    public float rotationSpeed = 2.0f; 
    //time it takes to lock onto the player
    public float lockOnTime = 10.0f; 
    //delay between each burst
    public float burstDelay = 1.0f; 
    //number of shots per shooting cycle
    public int burstCount = 3;
    
    //position from which bullets are fired
    public Transform gunBarrel; 

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 30.0f; 
    public float damagePerShot = 10.0f; 
    
    private bool _isPlayerInRange;
    private bool _isShooting;
    private bool _isLockedOn;
    private float _currentLockOnTime = 0f;


    private void Update()
    {
        if (_isPlayerInRange)
        {
            RotateTowardsTarget();

            //handle locking onto the player
            _currentLockOnTime += Time.deltaTime;
            if (_currentLockOnTime >= lockOnTime && !_isShooting)
            {
                StartCoroutine(FireBursts());
            }
        }
    }

    private void RotateTowardsTarget()
    {
        if (target == null) return;

        //calc direction towards the target
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        
        //rotate turret
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private IEnumerator FireBursts()
    {
        _isShooting = true;

        for (int i = 0; i < burstCount; i++)
        {
            FireBurst();
            yield return new WaitForSeconds(burstDelay);
        }
        
        DeactivateTurret();
    }

    private void FireBurst()
    {
        if (target == null) return;
        
        if (Physics.Raycast(gunBarrel.position,
                (target.position - gunBarrel.position).normalized,
                out RaycastHit hit,
                visionRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                // Fire bullet towards the player
                GameObject bullet = Instantiate(bulletPrefab, gunBarrel.position, Quaternion.identity);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.velocity = (target.position - gunBarrel.position).normalized * bulletSpeed;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
            _isShooting = false;
            _currentLockOnTime = 0f; 
        }
    }

    void DeactivateTurret()
    {
        _isShooting = false;
        _isPlayerInRange = false;
        _currentLockOnTime = 0f;
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }
}