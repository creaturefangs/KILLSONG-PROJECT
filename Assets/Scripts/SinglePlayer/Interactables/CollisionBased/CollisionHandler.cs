using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private CPlayerMovement playerMovement;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SlowSurface"))
        {
            // Reduce player's movement speed
            playerMovement.walkSpeed = 1f;
            Debug.Log("Collision with slowing object");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SlowSurface"))
        {
            // Reduce player's movement speed
            playerMovement.walkSpeed = 5f;
            Debug.Log("Exit collision with slowing object");
        }
    }   
}
