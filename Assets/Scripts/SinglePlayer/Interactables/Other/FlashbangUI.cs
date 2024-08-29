using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashbangUI : MonoBehaviour
{
    public Image flashOverlay;
    public float maxFadeDuration = 1f;
    private Color _overlayColor;

    public float initialFlashOpacity;
    private Flashbang _flashbang;
    private float _effectStrength;
    
    [SerializeField] private float delayBeforeFadeMultiplier = .5f;
    private void Awake()
    {
        _flashbang = GetComponent<Flashbang>();
    }

    public void StartFlash(float effectStrength)
    {
        _effectStrength = effectStrength; 

        if (_flashbang.flashbangCanvas != null)
        {
            _flashbang.flashbangCanvas.SetActive(true);
        }

        if (flashOverlay != null)
        {
            flashOverlay.gameObject.SetActive(true);
        }
        
        //calculate how long the canvas should stay active using effect strength
        float activeDuration = CalculateActiveDuration() * 1.5f;

        _overlayColor = flashOverlay.color; 
        
        float calculatedAlpha = Mathf.Clamp01((effectStrength / 10f) * 2f);
        _overlayColor.a = Mathf.Max(calculatedAlpha, 0.1f);

        flashOverlay.color = _overlayColor;
        
        float delayBeforeFade = Mathf.Lerp(0f, activeDuration, Mathf.Clamp01(effectStrength / 10f) * delayBeforeFadeMultiplier);
        
        StartCoroutine(FadeFlash(activeDuration, delayBeforeFade, _overlayColor.a));
    }

    private IEnumerator FadeFlash(float activeDuration, float delayBeforeFade, float initialAlpha)
    {
        yield return new WaitForSeconds(delayBeforeFade);

        float fadeDuration = activeDuration - delayBeforeFade;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            //fade out the flashbang overlay image
            _overlayColor.a = Mathf.Lerp(initialAlpha, 0f, elapsed / fadeDuration);
            flashOverlay.color = _overlayColor;
            yield return null;
        }
        
        _overlayColor.a = 0f;
        flashOverlay.color = _overlayColor;
        
        if (_flashbang.flashbangCanvas != null)
        {
            _flashbang.flashbangCanvas.SetActive(false);
        }
    }

    private float CalculateActiveDuration()
    {
        //using the scriptable object detonationEffectTime as a base duration for how long the effect should last
        return _flashbang.throwable.detonationEffectTime * Mathf.Clamp(_effectStrength / 10f, 1f, 5f); 
    }
}
