using UnityEngine;
using UnityEngine.Events;

public class Surface : CollisionBasedInteractableObject
{
    [HideInInspector] public float originalPlayerWalkSpeed;
    [HideInInspector] public float originalPlayerSprintSpeed;
    public float surfaceSpeedValueMultiplier = .5f;

    private new void Awake()
    {
        base.Awake();
        
        originalPlayerWalkSpeed = playerMovement.walkSpeed;
        originalPlayerSprintSpeed = playerMovement.sprintSpeed;
    }
}
