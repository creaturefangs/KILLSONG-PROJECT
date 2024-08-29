using UnityEngine;

public class FlashbangLight : MonoBehaviour
{
    public Light flashLight;
    public float maxFlashDuration = 1f;
    public float maxFlashIntensity = 50f;
    private float _flashDuration; 
    private float _initialIntensity; 
    private float _flashTimer = 0f; 
    private bool _isFlashing = false; 

    private void Start()
    {
        if (flashLight != null)
        {
            flashLight.enabled = false;
        }
    }

    public void SetFlashIntensityAndDuration(float effectStrength)
    {
        _flashDuration = maxFlashDuration * Mathf.Clamp(effectStrength / 10f, 0.5f, 1f);
        _initialIntensity = maxFlashIntensity * effectStrength;
        flashLight.intensity = _initialIntensity;
        _flashTimer = 0f;
        
        if (flashLight != null)
        {
            flashLight.enabled = true;
            _isFlashing = true;
        }
    }

    private void Update()
    {
        if (!_isFlashing) return;

        _flashTimer += Time.deltaTime;
        
        if (flashLight != null)
        {
            flashLight.intensity = Mathf.Lerp(_initialIntensity, 0f, _flashTimer / _flashDuration);
        }
        
        if (_flashTimer >= _flashDuration)
        {
            if (flashLight != null)
            {
                flashLight.enabled = false; 
            }
            _isFlashing = false; 
        }
    }
}
