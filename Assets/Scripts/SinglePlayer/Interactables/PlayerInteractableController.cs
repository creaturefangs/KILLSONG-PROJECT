using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractableController : MonoBehaviour
{   
    public KeyCode interactKey;
    
    //how long in between interactions
    [SerializeField] private float interactionDelay;
    [HideInInspector] public CollisionBasedInteractableObject currentInteractableObject;


    void Start(){
        
    }
    private void Update(){
        if(currentInteractableObject == null) return;

        if(Input.GetKeyDown(interactKey)){  
            currentInteractableObject.Interact();  
            print("interacted with " + currentInteractableObject.interactableData.name);
        }
    }
}
