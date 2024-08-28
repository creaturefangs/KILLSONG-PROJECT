using UnityEngine;

public class DoorController : CollisionBasedInteractableObject
{
    private Animator _doorAnimator;
    public int requiredKeyID;
    private PlayerInventory _playerInventory;
    
    private void OpenDoor()
    {
        if (_playerInventory.GetInventoryItemById(PlayerInventory.InventoryTypes.Hidden, requiredKeyID))
        {
            
        }
    }

    private void CloseDoor()
    {
        _doorAnimator.SetBool("Opened", false);
    }
}
