using UnityEngine;

public class CBIO_Door : CollisionBasedInteractableObject
{
    private Animator _doorAnimator;
    public int requiredKeyID;
    private PlayerInventory _playerInventory;
    private bool _opened;
    private void ToggleDoor()
    {
        if (_playerInventory.GetInventoryItemById(PlayerInventory.InventoryTypes.Hidden, requiredKeyID))
        {
            switch(_opened)
            {
                case false:
                    _doorAnimator.SetBool("DoorOpen", true);
                    break;
                case true:
                    _doorAnimator.SetBool("DoorOpen", false);
                    break;
            }
        }
        else
        {
            Debug.Log("Player does not have required key ID: " + requiredKeyID);
        }
    }

    private void CloseDoor()
    {
        
    }
}
