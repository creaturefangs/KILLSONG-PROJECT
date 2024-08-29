using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100;
    public float currentHealth;
    public GameObject damageUI;
    public GameObject deathScrn;
    public Text deathText;
    public string deathMessage = "[ VITALS OFFLINE ]";

    private GameObject overlay;
    //private DevTools devtools;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        overlay = GameObject.Find("BlinkOverlay");
        //devtools = GameObject.Find("PlayerController").GetComponent<DevTools>();

        // Make sure the death screen is initially inactive
        deathScrn.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateDamage();
    }

    void UpdateDamage()
    {
        Color currentAlpha = damageUI.GetComponent<RawImage>().color;
        currentAlpha.a = 1 - (currentHealth / maxHealth);
        damageUI.GetComponent<RawImage>().color = currentAlpha;
    }

    public void RestoreHealth(int amount)
    {
        currentHealth += amount;
        // Ensure the current health doesn't exceed the maximum health.
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        overlay.GetComponent<Animator>().Play("HealOverlay");
    }

    public void Die()
    {
        StartCoroutine(DeathSequence());
        Debug.Log( "You have Died");
    }

    IEnumerator DeathSequence()
    {
        // Activate the death screen
        deathScrn.SetActive(true);

        // Wait for 6 seconds
        yield return new WaitForSeconds(6f);

        
        // Type out the death message letter by letter
        yield return StartCoroutine(TypeText(deathMessage, deathText));

        // Deactivate the death screen
        deathScrn.SetActive(false);

        // Perform other actions like spectating or respawning
        // ...

        // Example: Respawn the player (you would replace this with your respawn logic)
        Respawn();

        // End of the coroutine
    }

    IEnumerator TypeText(string message, Text textComponent)
    {
        textComponent.text = "";
        foreach (char letter in message)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(0.05f); // Adjust the typing speed here
        }
    }

    void Respawn()
    {
        
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
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
}
