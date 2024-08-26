using UnityEngine;
using UnityEngine.Events;

public class DamageInstigator : MonoBehaviour
{
    //What tag should the other object have to inflict damage?
    public string triggerTagCheck = "Player";
    public bool applyDamageOverTime;
    public float damageAmount = 1.0f;
    public float damageTickMultiplier = 1.0f;
    
    private bool _inDamageArea;

    public UnityEvent onDamageStart;
    public UnityEvent onDamageEnd;
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(triggerTagCheck)) return;

        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
        _inDamageArea = true;

        if (damagable == null || !_inDamageArea) return;
        
        if (applyDamageOverTime)
        {
            damagable.TakeDamageOverTime(damageAmount, damageTickMultiplier); 
        }
        else
        {
            damagable.TakeDamage(damageAmount);
        }
        
        onDamageStart?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag(triggerTagCheck)) return;
        
        _inDamageArea = false;
        
        onDamageEnd?.Invoke();
    }
}
