using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CHealthUI : MonoBehaviour
{
    // Reference to the Health component
    public CPlayerHealth health;

    // Reference to the UI Image for the fill
    public Image fillImage;

    private void Start()
    {
        // Ensure that the health and fillImage references are set
        if (health == null)
        {
            Debug.LogError("Health reference is not set in " + gameObject.name);
            return;
        }

        if (fillImage == null)
        {
            Debug.LogError("Fill Image reference is not set in " + gameObject.name);
            return;
        }

        // Set the initial fill amount based on current health
        UpdateHealthUI();
    }

    private void Update()
    {
        // Continuously update the health UI to reflect current health
        UpdateHealthUI();
    }

    // Update the fill image based on the current health
    private void UpdateHealthUI()
    {
        // Calculate the fill amount percentage
        float fillAmount = (float)health.currentHealth / health.maxHealth;

        // Set the fill amount of the fill image
        fillImage.fillAmount = fillAmount;
    }
}
