using UnityEngine;

public class Flashbang : Throwable
{
    public GameObject flashbangCanvas;
    private FlashbangUI _flashbangUI;
    private FlashbangLight _flashbangLight;

    private void Awake()
    {
        _flashbangLight = GetComponent<FlashbangLight>();
        _flashbangUI = GetComponent<FlashbangUI>();

        if (flashbangCanvas != null)
        {
            flashbangCanvas.SetActive(false);
        }
    }

    public override void Detonate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, throwable.effectRadius);

        foreach (var hitCollider in hitColliders)
        {
            IDamageable damageable = hitCollider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damageAmount); 
            }

            //calculate effect strength based on distance between this and the damageable
            float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
            float effectStrength = (throwable.effectRadius - distance) / throwable.effectRadius * 10f;

            if (hitCollider.CompareTag("Player"))
            {
                ApplyFlashbangEffect(effectStrength);
            }
        }
        
        GameObject detonationVFX = Instantiate(throwable.prefabDetonationVfx, transform.position, Quaternion.identity);
    
        onDetonation?.Invoke();
        DestroyAfterTime(gameObject, throwable.postDetonationDestructionTime);
        DestroyAfterTime(detonationVFX, 0.0f);
    }

    private void ApplyFlashbangEffect(float effectStrength)
    {
        if (flashbangCanvas != null)
        {
            flashbangCanvas.SetActive(true);
        }

        if (_flashbangLight != null)
        {
            _flashbangLight.gameObject.SetActive(true);
            _flashbangLight.SetFlashIntensityAndDuration(effectStrength);
        }
 
        if (_flashbangUI != null)
        {
            _flashbangUI.gameObject.SetActive(true);
            _flashbangUI.StartFlash(effectStrength);
        }
    }
}