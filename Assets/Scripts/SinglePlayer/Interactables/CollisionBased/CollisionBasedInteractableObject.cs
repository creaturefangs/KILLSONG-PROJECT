using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CollisionBasedInteractableObject : MonoBehaviour, IInteractable
{
    public SO_CollisionBasedIO interactableData;
    
    [Tooltip("Interaction Event.")]
    public UnityEvent onInteractionEvent;
    [Tooltip("End Interaction Event.")]
    public UnityEvent onEndInteractionEvent;
    [Tooltip("Is this item a pickup?")]
    public bool isPickup;
    [Tooltip("Should this interactable get destroyed on interaction?")]
    public bool destroyOnInteraction;
    [Tooltip("Set if this object can be interacted with.")]
    [HideInInspector]
    public bool canInteract = false;
    [Tooltip("Tag to check for trigger events, default is Player.")]
    public string triggerObjectTagCheck = "Player";
    
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public PlayerInventory playerInventory;
    [HideInInspector]
    public CPlayerMovement playerMovement; 
    private PlayerUI _playerUI;
    
    public void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerMovement = FindObjectOfType<CPlayerMovement>();
        _playerUI = FindObjectOfType<PlayerUI>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(triggerObjectTagCheck)) return;
        
        canInteract = true;
        player.GetComponent<PlayerInteractableController>().currentInteractableObject = this;
       
        //Display interaction in UI
        _playerUI.DisplayInteractionUI(KeyCode.E, interactableData.interactionType, interactableData.interactableStruct);
    }

    public void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag(triggerObjectTagCheck)) return;

        canInteract = false;
        player.GetComponent<PlayerInteractableController>().currentInteractableObject = null;

        //Clear interaction UI
        _playerUI.ClearInteractionDisplay();
        
        onEndInteractionEvent?.Invoke();
    }

    public void Interact()
    {
        if(canInteract)
        {
            onInteractionEvent?.Invoke();
            if(isPickup)
            {
                OnPickup();
            }
        }
    }

    public void OnPickup()
    {
        if(destroyOnInteraction)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySoundOnInteraction(AudioSource source, AudioClip sound)
    {

    }
}
