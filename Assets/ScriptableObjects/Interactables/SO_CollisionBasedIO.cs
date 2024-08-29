using UnityEngine;

[CreateAssetMenu(menuName = "CollisionBasedInteractable", order = 0)]
public class SO_CollisionBasedIO : ScriptableObject
{
    [Tooltip("Struct containing data for the interactable object")]
    public STRInteractables interactableStruct;

    [Tooltip("Type of interaction (e.g., click, hold) for this interactable")]
    public STRInteractables.EInteractionType interactionType;

    [Tooltip("Duration the player must hold to interact (0 for instant interaction)")]
    [Range(0, 5)] 
    public float interactionHoldDuration;
}