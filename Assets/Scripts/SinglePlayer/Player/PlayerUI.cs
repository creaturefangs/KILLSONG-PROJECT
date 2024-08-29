using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private PlayerInteractableController playerInteractableController;

    public TMP_Text interactionText;
    public TMP_Text noteText;
    public TMP_Text dnaCountText;
    public GameObject notePanel;

    // Reference to the interaction key sprite
    public GameObject interactionKeyImage;
    // Reference to the Image component that will act as the circular progress bar
    public Image interactionProgressImage;

    private void Start()
    {
        ClearInteractionDisplay();
    }

#region Interaction UI
    public void DisplayInteractionUI(KeyCode interactionKey, STRInteractables.EInteractionType eInteractionType, STRInteractables interactStruct)
    {
        if (playerInteractableController == null || playerInteractableController.currentInteractableObject == null)
        {
            Debug.LogWarning("DisplayInteractionUI skipped: playerInteractableController or currentInteractableObject is null.");
            return;
        }

        
        //if the desired hold duration is less or = to 0, do not dispaly the interactionkeyimage
        if (playerInteractableController.currentInteractableObject.interactableData.interactionHoldDuration <=
            0) return;
        
        // Show interaction UI elements
        interactionKeyImage.SetActive(true);
        interactionProgressImage.gameObject.SetActive(true);

        string displayText;
        
        switch (eInteractionType)
        {
            case STRInteractables.EInteractionType.BasicInteraction:
                displayText = playerInteractableController.currentInteractableObject.isPickup
                    ? $"Hold    {GetInteractionIconPlaceholder()}to pickup {interactStruct.interactableName}"
                    : $"Hold    {GetInteractionIconPlaceholder()}to interact with {interactStruct.interactableName}";
                break;
            default:
                displayText = string.Empty;
                break;
        }

        interactionText.text = displayText;
        //Debug.Log("Interaction text set to: " + displayText);

        // Update the layout so the text info is ready to be used
        interactionText.ForceMeshUpdate();

        // Reposition the interaction key icon over the placeholder
        PositionIconsOverPlaceholder();
    }

    private string GetInteractionIconPlaceholder()
    {
        // Return a string that acts as a placeholder for the icon position
        return "{key}";
    }

    private void PositionIconsOverPlaceholder()
    {
        //Get the index of the placeholder in the interaction text
        int placeholderIndex = interactionText.text.IndexOf("{key}");
        if (placeholderIndex >= 0)
        {
            //determine the position of the character
            TMP_TextInfo textInfo = interactionText.textInfo;

            if (textInfo.characterCount > placeholderIndex)
            {
                //Get the character info for the start of the placeholder
                TMP_CharacterInfo charInfo = textInfo.characterInfo[placeholderIndex];

                //Calc world position for the key icon
                Vector3 worldPos = interactionText.transform.TransformPoint((charInfo.bottomLeft + charInfo.topRight) / 2);

                //Position the images where the placeholder is
                interactionKeyImage.transform.position = worldPos;
                interactionProgressImage.transform.position = worldPos;

                //Avoid showing '{key}', replace with empty string
                interactionText.text = interactionText.text.Replace("{key}", ""); // Space for the icon
            }
        }
    }

    public void ClearInteractionDisplay()
    {
        interactionText.text = "";
        //Debug.Log("Interaction display cleared.");

        interactionKeyImage.SetActive(false);
        interactionProgressImage.gameObject.SetActive(false);
        interactionProgressImage.fillAmount = 0;
    }
#endregion
}
