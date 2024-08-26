using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CNightVisController : MonoBehaviour
{

    public PostProcessVolume nightVisionVolume; // Reference to the Night Vision Post-Process Volume
    public PostProcessVolume mainPPVolume; // Reference to the Main Post-Process Volume
    private bool isNightVisionActive = false;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip nightVisSFX;

    void Start()
    {
        // Ensure the night vision effect is initially off and main PP effect is on
        nightVisionVolume.enabled = false;
        mainPPVolume.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Toggle the night vision and main PP volumes
            isNightVisionActive = !isNightVisionActive;
            nightVisionVolume.enabled = isNightVisionActive;
            mainPPVolume.enabled = !isNightVisionActive;

            if (isNightVisionActive)
            {
                // Play the night vision sound effect when night vision is activated
                if (audioSource != null && nightVisSFX != null)
                {
                    audioSource.PlayOneShot(nightVisSFX);
                }
            }
            else
            {
                // Stop the audio source when the main post-processing volume is active
                if (audioSource != null)
                {
                    audioSource.Stop();
                }
            }
        }
    }
}
