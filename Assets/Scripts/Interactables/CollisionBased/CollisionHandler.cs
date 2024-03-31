using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionHandler : MonoBehaviour
{
    // Damage to be applied on collision
    public int damageAmount = 10;

    // Rate of damage per second
    public float damagePerSecond = 5f;

    // Reference to the player object
    // Reference to the ui elements
    public GameObject player;

    public GameObject doorlockPanel;
    public GameObject noteTxt;
    public GameObject notePanel;

    // audio feedback
    public AudioClip healthpickupSFX;
    public AudioClip doorlockSFX;
    public AudioClip noteSFX;
    public AudioClip grenadeSFX;
    public AudioSource audioSource;
    private float volume = 1.0f;

    // Health component of the player object
    private CPlayerHealth playerHealth;

    // speed component of the player object
    private CPlayerMovement playerMovement;

    // Flag to track if the player is in a damage area
    private bool isInDamageArea = false;

    // Delay between each damage application for damage over time
    private float damageInterval = 3f; // Apply damage every 3 second

    // Animation component for grenade explosion
    public Animation grenadeAnimation;

  


    private void Start()
    {
        // Attempt to get the Health component attached to the player object
        playerHealth = player.GetComponent<CPlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogWarning("No CPlayerHealth component found on player object.");
        }
    }

    private void Update()
    {
        // If the player is in a damage area, apply damage over time
        if (isInDamageArea)
        {
            ApplyDamageOverTime();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DamageBox"))
        {
            // Apply damage to the player
            ApplyDamage(other.gameObject);
            // Disable the damage box collider
            other.gameObject.SetActive(false);
            Debug.Log("Collision with damage box");
            //audio feedback is handled in player health script 
        }
        else if (other.CompareTag("DamageArea"))
        {
            // Set flag to indicate that the player is in a damage area
            isInDamageArea = true;
            Debug.Log("Entered damage area");
        }
        else if (other.CompareTag("HealthPickup"))
        {
            // Heal the player
            playerHealth.Heal(25);
            // Disable the health pickup collider
            other.gameObject.SetActive(false);
            Debug.Log("Collision with health pickup");
            // audio feedback
            audioSource.PlayOneShot(healthpickupSFX, volume);

        }
        else if (other.CompareTag("DoorLock"))
        {
            doorlockPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("entered doorlock");
            //audio feedback
            audioSource.PlayOneShot(doorlockSFX, volume);
        }
        else if (other.CompareTag("Notes"))
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
        }
        else if (other.CompareTag("Grenade"))
        {
            // Play grenade explosion animation and sound after 5 seconds
            Invoke("PlayExplosion", 5f);
            // Disable the grenade collider
            other.gameObject.SetActive(false);
            Debug.Log("Collision with grenade");
        }
        else if (other.CompareTag("SlowingObject"))
        {
            // Reduce player's movement speed
            playerMovement.speed = 1f;
            Debug.Log("Collision with slowing object");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DamageArea"))
        {
            // Reset flag when the player exits the damage area
            isInDamageArea = false;
            Debug.Log("Exited damage area");
        }
        else if (other.CompareTag("DoorLock"))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            doorlockPanel.SetActive(false);
            Debug.Log("Exited doorlock");
        }
        else if (other.CompareTag("Notes"))
        {
            noteTxt.SetActive(false);
        }
        else if (other.CompareTag("SlowingObject"))
        {
            // Reduce player's movement speed
            playerMovement.speed = 5f;
            Debug.Log("Exit collision with slowing object");
        }
    }

    // Apply damage to the player
    private void ApplyDamage(GameObject target)
    {
        // Check if the player has a health component
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
        else
        {
            Debug.LogWarning("No CPlayerHealth component found on player object. Damage not applied.");
        }
    }

    // Apply damage over time to the player while in a damage area
    private void ApplyDamageOverTime()
    {
        // Only apply damage if the delay interval has passed
        if (Time.time >= damageInterval)
        {
            // Calculate damage for this frame
            float damageThisFrame = damagePerSecond * Time.deltaTime;

            // Apply damage to the player
            ApplyDamage(player);

            // Increase the next damage interval by the delay
            damageInterval = Time.time + 3f / damagePerSecond;

            Debug.Log("Damage over time: " + damageThisFrame);
        }
    }

    // Play grenade explosion animation and sound
    private void PlayExplosion()
    {
        // Check if the grenade animation and explosion sound are assigned
        if (grenadeAnimation != null && grenadeSFX != null)
        {
            // Play grenade explosion animation
            grenadeAnimation.Play();

            audioSource.PlayOneShot(grenadeSFX, volume);
        }
        else
        {
            Debug.LogWarning("Grenade animation or explosion sound not assigned.");
        }
    }
}
