using TMPro;
using UnityEngine;


public class CollisionHandler : MonoBehaviour
{
    public GameObject player;

    public GameObject noteTxt;
    public GameObject notePanel;

    public AudioClip noteSFX;

    public AudioSource audioSource;
    private float volume = 1.0f;

    public Animator doorController; 

    private CPlayerMovement playerMovement;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Notes"))
        {
            noteTxt.SetActive(true);
            Debug.Log("note collision");

            // audio feedback
            audioSource.PlayOneShot(noteSFX, volume);

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log(" e key is pressed");
                noteTxt.SetActive(false);
                notePanel.SetActive(true);
            }

            else if (other.CompareTag("SlowSurface"))
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
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Notes"))
        {
            noteTxt.SetActive(false);
        }
        else if (other.CompareTag("SlowSurface"))
        {
            // Reduce player's movement speed
            playerMovement.speed = 5f;
            Debug.Log("Exit collision with slowing object");
        }
    }   
}
