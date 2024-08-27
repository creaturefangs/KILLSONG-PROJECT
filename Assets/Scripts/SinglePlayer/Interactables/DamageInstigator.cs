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

    public UnityEvent onDamageStart;
    public UnityEvent onDamageEnd;

    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(triggerTagCheck)) return;

        if (isTrigger && canDamage)
        {
            IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
            _inDamageArea = true;

            if (damagable == null || !_inDamageArea) return;

            switch(damageType)
            {
                case DamageType.Instant:
                    damagable.TakeDamage(damageAmount);
                    break;
                case DamageType.OverTime:
                    if (_damageCoroutine == null)
                    {
                        _damageCoroutine = StartCoroutine(ApplyDamageOverTime(damagable));
                    }
                    break;
                default:
                    damagable.TakeDamage(damageAmount);
                    break;
            }

            onDamageStart?.Invoke();
        }
        
        if (disableAfterOneUse)
        {
            canDamage = false;
        }
    }

    private IEnumerator ApplyDamageOverTime(IDamagable damagable)
    {
        while (_inDamageArea)
        {
            damagable.TakeDamageOverTime(damageAmount, damageTickMultiplier);
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