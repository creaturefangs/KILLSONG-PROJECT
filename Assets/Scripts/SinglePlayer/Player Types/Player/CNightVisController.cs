using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CNightVisController : MonoBehaviour
{
    public PostProcessVolume nightVisionVolume; // Reference to the Post-Process Volume
    public GameObject mainPPVolume; 
    private bool isNightVisionActive = false;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip nightVisSFX;
    void Start()
    {
        // Ensure the night vision effect is initially off
        nightVisionVolume.enabled = false;
        mainPPVolume.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isNightVisionActive = !isNightVisionActive;
            nightVisionVolume.enabled = isNightVisionActive;
            mainPPVolume.SetActive(false);
            audioSource.PlayOneShot(nightVisSFX);
        }
    }
}
