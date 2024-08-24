using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CPlayerHealth : MonoBehaviour
{
    // Respawn Screen
    public GameObject playerDeath;

    // audio feedback for taking damage 
    public AudioClip takeDamageSFX;
    public AudioClip deathSFX;
    public AudioSource audioSource;
    private float volume = 1.0f;
    private float volumeDeath = 0.2f;

    // Current health value
    public int currentHealth = 100;

    // Maximum health value
    public int maxHealth = 100;

    // Event triggered when the object dies
    public event System.Action OnDeath;

    // Method to take damage
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        audioSource.PlayOneShot(takeDamageSFX, volume);

        // Check if health has reached zero or below
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    // Method to heal
    public void Heal(int amount)
    {
        currentHealth += amount;
        // Ensure health doesn't exceed maximum
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    // Method to handle death
    private void Die()
    {
        // Trigger the death event
        OnDeath?.Invoke();

        //Play death sound
        audioSource.PlayOneShot(deathSFX, volumeDeath);

        // Optionally, you can disable the gameObject, play death animations, etc.
        gameObject.SetActive(false);
        playerDeath.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //method to handle button respawn 
    public void Respawn()
    {
        gameObject.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
