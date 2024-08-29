using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Throwable : DamageInstigator
{
    public SO_Throwable throwable;
    public UnityEvent onDetonation;
    private float _detonationTime;
    public bool hasRandomDetonationTime;

    public bool enableDrawGizmosDebug = true;
    private void Start()
    {
        //get a random detonation time if hasRandomDetonationTime is true, otherwise default to the maxDetonationTime
        _detonationTime = hasRandomDetonationTime 
            ? Random.Range(throwable.minDetonationTime, throwable.maxDetonationTime) 
            : throwable.maxDetonationTime;

        Invoke(nameof(Detonate), _detonationTime);
    }

    public virtual void Detonate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, throwable.effectRadius);

        foreach (var hitCollider in hitColliders)
        {
            IDamageable damageable = hitCollider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damageAmount);
            }
        }

        GameObject detonationVFX = Instantiate(throwable.prefabDetonationVfx, transform.position, Quaternion.identity);
        
        onDetonation?.Invoke();
        DestroyAfterTime(gameObject, throwable.postDetonationDestructionTime);
        DestroyAfterTime(detonationVFX, throwable.postDetonationDestructionTime);
    }

    public void DestroyAfterTime(GameObject goToDestroy, float timeToDestroy)
    {
        StartCoroutine(DestroyRoutine(goToDestroy, timeToDestroy));
    }

    private IEnumerator DestroyRoutine(GameObject goToDestroy, float timeToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(goToDestroy);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (enableDrawGizmosDebug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, throwable.effectRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, throwable.projectileThrowRange);
        }
    }
}