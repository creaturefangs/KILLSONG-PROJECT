using UnityEngine;

public class CBIO_DoorLock : CollisionBasedInteractableObject
{
    [SerializeField] private GameObject doorLockPanel;
    [SerializeField] private AudioClip doorLockSFX;

    public void HandleDoorLock(bool active)
    {
        doorLockPanel.SetActive(active);
        EnvironmentalSoundController.Instance.PlaySoundAtLocation(doorLockSFX, 1.0f, transform.position);
        if (active)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    
}
