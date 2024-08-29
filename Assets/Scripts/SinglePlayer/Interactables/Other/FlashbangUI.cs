using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashbangUI : MonoBehaviour
{
    public Image flashOverlay;
    public float fadeDuration = 1f;
    private Color overlayColor;

    void Start()
    {
        overlayColor = flashOverlay.color;
        overlayColor.a = 1f; // Start fully opaque
        flashOverlay.color = overlayColor;
    }

    void Update()
    {
        overlayColor.a = Mathf.Lerp(1f, 0f, Time.time / fadeDuration);
        flashOverlay.color = overlayColor;

        if (overlayColor.a <= 0f)
        {
            Destroy(gameObject); // Remove the overlay after the flash
        }
    }
}
