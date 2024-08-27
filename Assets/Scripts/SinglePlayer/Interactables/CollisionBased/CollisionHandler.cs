using UnityEngine;


public class CollisionHandler : MonoBehaviour
{
    public Animator doorController; 

    private CPlayerMovement playerMovement;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SlowSurface"))
        {
            // Reduce player's movement speed
            playerMovement.speed = 1f;
            Debug.Log("Collision with slowing object");
        }
        else if (other.CompareTag("DoorTouch"))
        {
            // open the door via animation
            doorController.SetBool("DoorOpen", true);
            Debug.Log("Collision with door");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SlowSurface"))
        {
            // Reduce player's movement speed
            playerMovement.speed = 5f;
            Debug.Log("Exit collision with slowing object");
        }
    }   
}
