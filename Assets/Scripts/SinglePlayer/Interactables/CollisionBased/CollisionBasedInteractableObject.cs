using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionBasedInteractableObject : MonoBehaviour, IInteractable
{
    public STRInteractables interactableStruct;
   
    [Tooltip("Interaction Event.")]
    public UnityEvent onInteractionEvent;
    [Tooltip("Should this interactable get destroyed on interaction?")]
    public bool destroyOnInteraction;
    [Tooltip("Set if this object can be interacted with.")]
     public bool canInteract = false;
    [Tooltip("Tag to check for trigger events, default is Player.")]
     public string triggerObjectTagCheck = "Player";
    
    [HideInInspector]
    public GameObject player;

    private void Awake(){
        player = GameObject.FindWithTag("Player");
    }
    public void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(triggerObjectTagCheck)) return;

        canInteract = true;
    }

    public void Interact(){
        if(canInteract){
            onInteractionEvent?.Invoke();
        }
    }

    public void OnPickup(){
        if(destroyOnInteraction){
            Destroy(gameObject);
        }
    }
}