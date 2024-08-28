using System.Collections;
using UnityEngine;

public class CBIO_Door : CollisionBasedInteractableObject
{
    private Animator _doorAnimator;
    public int requiredKeyID;
    private bool _opened;
    private bool _onCooldown;
    private const float CooldownTime = 2.0f;

    private new void Awake()
    {
        base.Awake();
        _doorAnimator = GetComponentInParent<Animator>();
    }
    public void ToggleDoor()
    {
        if (playerInventory.GetInventoryItemById(PlayerInventory.InventoryTypes.Hidden, requiredKeyID))
        {
            if (!_onCooldown)
            {
                _doorAnimator.SetBool("DoorOpen", !_opened);
                StartCoroutine(DoorCooldown());
            }
        }
        else
        {
            Debug.Log("Player does not have required key ID: " + requiredKeyID);
        }
    }

    private IEnumerator DoorCooldown()
    {
        _onCooldown = true;
        yield return new WaitForSeconds(CooldownTime);
        _onCooldown = false;
    }
}
