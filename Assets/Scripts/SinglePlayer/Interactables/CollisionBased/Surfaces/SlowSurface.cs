using UnityEngine;

public class SlowSurface : Surface
{
    private new void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(triggerObjectTagCheck)) return;
        base.OnTriggerEnter(other);
        
        TogglePlayerSpeed(true);
    }
    
    private new void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(triggerObjectTagCheck)) return;
        base.OnTriggerExit(other);
        
        TogglePlayerSpeed(false);
    }
    
    private void TogglePlayerSpeed(bool onSurface)
    {
        // Always adjust both walk and sprint speeds regardless of the sprint state
        playerMovement.walkSpeed = onSurface ? originalPlayerWalkSpeed * 0.5f : originalPlayerWalkSpeed;
        playerMovement.sprintSpeed = onSurface ? originalPlayerSprintSpeed * 0.25f : originalPlayerSprintSpeed;
    }
}