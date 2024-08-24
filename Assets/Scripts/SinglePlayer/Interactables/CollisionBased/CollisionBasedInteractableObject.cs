using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionBasedInteractableObject : MonoBehaviour, IInteractable
{
    [Tooltip("Interaction Event.")]public UnityEvent onInteraction;
    [Tooltip("Should this interactable get destroyed on pickup?")]public bool destroyOnPickup;
    [Tooltip("Set if this object can be interacted with.")] public bool canInteract = false;
    [Tooltip("Tag to check for trigger events, default is Player.")] public string triggerObjectTagCheck = "Player";
    public void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(triggerObjectTagCheck)) return;

        canInteract = true;
    }

    public void Interact(){
        if(canInteract){
            onInteraction?.Invoke();
        }
    }

    public void OnPickup(){

    }
}
