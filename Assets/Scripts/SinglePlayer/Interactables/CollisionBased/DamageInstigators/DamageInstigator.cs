using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DamageInstigator : MonoBehaviour
{
    public enum DamageType
    {
        Instant,
        OverTime
    }
    public DamageType damageType;
    public bool isTrigger;
    public bool disableAfterOneUse;
    public bool canDamage = true;

    public string triggerTagCheck = "Player";
    public float damageAmount = 1.0f;
    public float damageTickMultiplier = 1.0f;
    
    private bool _inDamageArea;
    private Coroutine _damageCoroutine;

    public GameObject damageEffect;
    public float damageEffectDestructionDelay = .5f;
    
    public UnityEvent onDamageStart;
    public UnityEvent onDamageEnd;

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
            if (damageEffect != null)
            {
                if (Physics.Raycast(
                        transform.position,
                        (other.transform.position - transform.position).normalized,
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

        if (disableAfterOneUse)
        {
            canDamage = false;
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
}
