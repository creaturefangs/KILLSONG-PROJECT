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
    //What tag should the other object have to inflict damage?
    public string triggerTagCheck = "Player";
    //How much damage should be inflicted?
    public float damageAmount = 1.0f;
    //How many times should the damage happen per second?
    public float damageTickMultiplier = 1.0f;
    
    private bool _inDamageArea;

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
                    damagable.TakeDamageOverTime(damageAmount, damageTickMultiplier);
                    break;
                //Default to instant damage
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

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag(triggerTagCheck)) return;

        if (isTrigger)
        {
            _inDamageArea = false;
        
            onDamageEnd?.Invoke();
            
        }
        
        if (disableAfterOneUse)
        {
            canDamage = false;
        }
    }
}
