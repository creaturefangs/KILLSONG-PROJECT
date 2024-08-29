using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashbangLight : MonoBehaviour
{
    public Light flashLight;
    public float flashDuration = 1f;
    private float timer = 0f;
    public AudioSource playerSFX;
    public AudioClip flashbangSFX;

    void Start()
    {
        flashLight.intensity = 50f; // Start with a high intensity
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Decrease the light intensity over time
        flashLight.intensity = Mathf.Lerp(50f, 0f, timer / flashDuration);

        if (timer >= flashDuration)
        {
            Destroy(gameObject); // Remove the light after the flash
        }
    }
}
