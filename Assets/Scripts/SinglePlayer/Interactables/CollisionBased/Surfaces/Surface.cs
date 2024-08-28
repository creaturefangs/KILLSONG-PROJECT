using UnityEngine;
using UnityEngine.Events;

public class Surface : CollisionBasedInteractableObject
{
    public UnityEvent onEnterSurface;
    public UnityEvent onExitSurface;
    public void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(triggerObjectTagCheck)) return;
        
        onEnterSurface?.Invoke();
    }
    // public new void OnTriggerExit(Collider other)
    // {
    //     base.OnTriggerExit(other);
    //     
    //     onExitSurface?.Invoke();
    // }
}
