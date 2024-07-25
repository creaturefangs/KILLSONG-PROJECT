using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CNightVisController : MonoBehaviour
{
    public PostProcessVolume nightVisionVolume; // Reference to the Post-Process Volume
    private bool isNightVisionActive = false;

    void Start()
    {
        // Ensure the night vision effect is initially off
        nightVisionVolume.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isNightVisionActive = !isNightVisionActive;
            nightVisionVolume.enabled = isNightVisionActive;
        }
    }
}
