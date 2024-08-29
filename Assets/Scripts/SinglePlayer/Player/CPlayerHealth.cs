using System;
using UnityEngine;


public class CPlayerHealth : MonoBehaviour, IDamageable
{
    // Respawn Screen
    public GameObject playerDeath;

    // audio feedback for taking damage 
    public AudioClip takeDamageSFX;
    public AudioClip deathSFX;
    public AudioSource audioSource;
    private float volume = 1.0f;
    private float volumeDeath = 0.2f;
    
    public float currentHealth = 100;
    
    public float maxHealth = 100;
    
    public event Action OnDeath;
    
    public void TakeDamage(float amount)
    {
        if (currentHealth > 0)
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
        
    }
    public void TakeDamageOverTime(float amount, float tickMultiplier)
    {
        if (currentHealth > 0)
        {
            currentHealth -= amount * (Time.deltaTime * tickMultiplier);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }
    
    public void Heal(float amount)
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
