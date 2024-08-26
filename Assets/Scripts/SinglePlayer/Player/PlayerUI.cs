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
    
    // public void DisplayInteractionUI(KeyCode interactionKey, CollisionBasedInteractableObject.EInteractionType eInteractionType, STRInteractables interactStruct)
    // {
    //     if (playerInteractableController == null || playerInteractableController.currentInteractableObject == null) return;
    //     switch(eInteractionType)
    //     {
    //         case CollisionBasedInteractableObject.EInteractionType.Interact:
    //             interactionText.text = playerInteractableController.currentInteractableObject.isPickup
    //                 ? interactionText.text = $"Press + {interactionKey} to pickup " + interactStruct.interactableName
    //                 : interactionText.text = $"Press + {interactionKey} to interact with " + interactStruct.interactableName;
    //             break;
    //     }
    // }
}
