using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractableController : MonoBehaviour
{   
    public KeyCode interactKey;
    

    [SerializeField] private float interactionDelay;
    public CollisionBasedInteractableObject _currentInteractableObject;

    private void Update(){
        if(_currentInteractableObject == null) return;

        if(Input.GetKeyDown(interactKey)){  
            _currentInteractableObject.Interact();  
        }
    }
}
