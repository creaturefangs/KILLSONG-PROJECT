using System;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private PlayerInteractableController playerInteractableController;
    
    
    public TMP_Text interactionText;
    public TMP_Text noteText;
    public TMP_Text dnaCountText;
    public GameObject notePanel;

    private void Start()
    {
        ClearInteractionDisplay();
    }

    public void DisplayInteractionUI(KeyCode interactionKey, STRInteractables.EInteractionType eInteractionType, STRInteractables interactStruct)
    {
        if (playerInteractableController == null)
        {
            Debug.LogWarning("DisplayInteractionUI skipped: playerInteractableController or ");
            return;
        }

        if (playerInteractableController.currentInteractableObject == null)
        {
            Debug.Log("currentInteractableObject is null.");
            return;
        }

        switch (eInteractionType)
        {
            case STRInteractables.EInteractionType.BasicInteraction:
                string displayText = playerInteractableController.currentInteractableObject.isPickup
                    ? $"Press {interactionKey} to pickup {interactStruct.interactableName}"
                    : $"Press {interactionKey} to interact with {interactStruct.interactableName}";
                interactionText.text = displayText;
                Debug.Log("Interaction text set to: " + displayText);
                break;
        }
    }


    public void ClearInteractionDisplay()
    {
        interactionText.text = "";
        Debug.Log("Interaction display cleared.");
    }

}
