using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DamageInstigator : MonoBehaviour
{
    public enum DamageType
    {
        Instant,
        OverTime,
        ParticleCollision
    }
    public DamageType damageType;
    public bool isTrigger;
    public bool disableAfterOneUse;
    public bool canDamage = true;

    public string triggerTagCheck = "Player";
    public float damageAmount = 1.0f;
    public float damageTickMultiplier = 1.0f;

    public GameObject damageEffect;
    public float damageEffectDestructionDelay = .5f;
    
    public UnityEvent onDamageStart;
    public UnityEvent onDamageEnd;
    
    private bool _inDamageArea;
    private Coroutine _damageCoroutine;
    
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(triggerTagCheck)) return;

        if (isTrigger && canDamage)
        {
            //if the entered trigger implements the IDamagable interface.
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            _inDamageArea = true;

            if (damageable == null) return;

            switch (damageType)
            {
                case DamageType.Instant:
                    damageable.TakeDamage(damageAmount);
                    break;
                case DamageType.OverTime:
                    _damageCoroutine ??= StartCoroutine(ApplyDamageOverTime(damageable));
                    break;
                default:
                    damageable.TakeDamage(damageAmount);
                    break;
            }

            onDamageStart?.Invoke();

            //spawn damage effect at hit point
            HandleSpawnDamageEffect(other.gameObject);
        }

        if (disableAfterOneUse)
        {
            canDamage = false;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag(triggerTagCheck)) return;

        if (isTrigger)
        {
            _inDamageArea = false;
            if (_damageCoroutine != null)
            {
                StopCoroutine(_damageCoroutine);
                _damageCoroutine = null;
            }

            onDamageEnd?.Invoke();
        }

        if (disableAfterOneUse)
        {
            canDamage = false;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!other.CompareTag(triggerTagCheck) || damageType != DamageType.ParticleCollision) return;

        Debug.Log("In particle collision with " + other.gameObject.name);

        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

        if (!canDamage) return;

        _inDamageArea = true; // Set this to true when a collision is detected

        if (damageable != null)
        {
            // Ensure the coroutine is running correctly
            _damageCoroutine ??= StartCoroutine(ApplyDamageOverTime(damageable));
        }

        onDamageStart?.Invoke();

        HandleSpawnDamageEffect(other.gameObject);

        if (disableAfterOneUse)
        {
            canDamage = false;
        }
    }

    private void OnParticleExit(GameObject other)
    {
        if (!other.CompareTag(triggerTagCheck)) return;
        
        if (_damageCoroutine != null)
        {
            StopCoroutine(_damageCoroutine);
            _damageCoroutine = null;
        }

        _inDamageArea = false;
        onDamageEnd?.Invoke();
    }

    private void HandleSpawnDamageEffect(GameObject hitObject)
    {
        if (damageEffect != null)
        {
            if (Physics.Raycast(
                    transform.position,
                    (hitObject.transform.position - transform.position).normalized,
                    out RaycastHit hitInfo,
                    15.0f))
            {
                GameObject spawnedEffect =
                    Instantiate(
                        damageEffect,
                        hitInfo.point,
                        Quaternion.LookRotation(hitInfo.normal));
                Destroy(spawnedEffect, damageEffectDestructionDelay);
            }
        }
    }

    private IEnumerator ApplyDamageOverTime(IDamageable damageable)
    {
        while (_inDamageArea)
        {
            damageable.TakeDamageOverTime(damageAmount, damageTickMultiplier);
            yield return new WaitForSeconds(1.0f / damageTickMultiplier);
        }
    }
}
