using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractableController : MonoBehaviour
{   
    //Whatever key should handle interaction events
    public KeyCode interactKey;
    
    //how long in between interactions
    [SerializeField] private float interactionDelay;
    //Keep track of the current interactable object
    [HideInInspector] public CollisionBasedInteractableObject currentInteractableObject;
    //To ensure that the interaction cooldown doesn't get stuck, let's keep track of it here.
    private Coroutine _currentInteractionCooldown;
    //Can the player interact?
    private bool _canInteract = true;
    
    //How long has the interaction key been held during this interaction?
    private float _currentInteractionHoldTime = 0f;

    [SerializeField] private PlayerUI playerUI;
    
     private void Awake()
     {
         // Ensure the progress image is hidden initially
        if (playerUI.interactionProgressImage != null)
        {
            playerUI.interactionProgressImage.fillAmount = 0f;
            playerUI.interactionProgressImage.enabled = false; // Hide the image initially
        }
     }

    private void Update()
    {
        if (currentInteractableObject == null || !_canInteract) return;

        if (Input.GetKey(interactKey))
        {
            // Increment the hold time while the key is held
            _currentInteractionHoldTime += Time.deltaTime;

            // Update the progress image fill amount
            if (playerUI.interactionProgressImage != null)
            {
                playerUI.interactionProgressImage.enabled = true; // Show the image while holding the key
                playerUI.interactionProgressImage.fillAmount = _currentInteractionHoldTime / currentInteractableObject.interactableData.interactionHoldDuration;
            }

            // Check if the hold time meets or exceeds the required duration
            if (_currentInteractionHoldTime >= currentInteractableObject.interactableData.interactionHoldDuration)
            {
                // Trigger the interaction
                currentInteractableObject.Interact();
                print("Interacted with " + currentInteractableObject.interactableData.name);
                
                // Start the interaction cooldown
                _currentInteractionCooldown = StartCoroutine(InteractionCooldown());
                //Clear interaction UI
                playerUI.ClearInteractionDisplay();
            }
        }
        else
        {
            // Reset the hold time and hide the progress image if the key is released
            _currentInteractionHoldTime = 0f;

            if (playerUI.interactionProgressImage != null)
            {
                playerUI.interactionProgressImage.fillAmount = 0f;
                playerUI.interactionProgressImage.enabled = false; // Hide the image when the key is released
            }
        }
    }

    private IEnumerator InteractionCooldown()
    {
        // Disable interaction during cooldown
        _canInteract = false;
        _currentInteractionHoldTime = 0f;  // Reset hold time for next interaction attempt
        if (playerUI.interactionProgressImage != null)
        {
            playerUI.interactionProgressImage.fillAmount = 0f;
            playerUI.interactionProgressImage.enabled = false; // Hide the image during cooldown
        }
        yield return new WaitForSeconds(interactionDelay);
        _canInteract = true;
    }

    public void ToggleInteractionKey()
    {
        playerUI.interactionKeyImage.SetActive(!playerUI.interactionKeyImage.activeSelf);
        playerUI.interactionProgressImage.gameObject.SetActive(!playerUI.interactionProgressImage.gameObject.activeSelf);
    }
}