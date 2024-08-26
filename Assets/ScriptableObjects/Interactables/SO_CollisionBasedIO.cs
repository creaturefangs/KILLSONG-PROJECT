using UnityEngine;

[CreateAssetMenu(menuName = "CollisionBasedInteractable", order = 0)]
public class SO_CollisionBasedIO : ScriptableObject
{
    public STRInteractables interactableStruct;
    public STRInteractables.EInteractionType interactionType;
}