using System;
using System.Collections;
using UnityEngine;

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

    private PlayerUI _playerUI;

    private void Awake()
    {
        _playerUI = GetComponentInChildren<PlayerUI>();
    }
    private void Update()
    {
        if (currentInteractableObject == null) return;

        if(Input.GetKeyDown(interactKey) && _canInteract){  
            currentInteractableObject.Interact();
            _currentInteractionCooldown = StartCoroutine(InteractionCooldown());
            print("interacted with " + currentInteractableObject.interactableData.name);
        }
    }

    private IEnumerator InteractionCooldown()
    {
        _canInteract = false;
        yield return new WaitForSeconds(interactionDelay);
        _canInteract = true;
    }
}
