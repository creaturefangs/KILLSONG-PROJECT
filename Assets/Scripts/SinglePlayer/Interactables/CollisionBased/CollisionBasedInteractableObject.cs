using UnityEngine;
using UnityEngine.Events;

public class CollisionBasedInteractableObject : MonoBehaviour, IInteractable
{
    public SO_CollisionBasedIO interactableData;
    
    [Tooltip("Interaction Event.")]
    public UnityEvent onInteractionEvent;
    [Tooltip("Is this item a pickup?")]
    public bool isPickup;
    [Tooltip("Should this interactable get destroyed on interaction?")]
    public bool destroyOnInteraction;
    [Tooltip("Set if this object can be interacted with.")]
    public bool canInteract = false;
    [Tooltip("Tag to check for trigger events, default is Player.")]
    public string triggerObjectTagCheck = "Player";
    
    [HideInInspector]
    public GameObject player;
    public PlayerUI playerUI;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerUI = FindObjectOfType<PlayerUI>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(triggerObjectTagCheck)) return;
        
        canInteract = true;
        player.GetComponent<PlayerInteractableController>().currentInteractableObject = this;
        print(canInteract);
        
        //Display in UI
        if (playerUI == null)
        {
            Debug.LogError("Playerui is null");
            return;
        }
        playerUI.DisplayInteractionUI(KeyCode.E, interactableData.interactionType, interactableData.interactableStruct);
    }

    public void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag(triggerObjectTagCheck)) return;

        canInteract = false;
        player.GetComponent<PlayerInteractableController>().currentInteractableObject = null;
        print(canInteract);
        
        //Clear interaction UI
        playerUI.ClearInteractionDisplay();
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
