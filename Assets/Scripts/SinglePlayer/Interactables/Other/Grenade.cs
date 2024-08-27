using UnityEngine;

public class Grenade : DamageInstigator
{
    [SerializeField] private Animation grenadeAnimation;
    [SerializeField] private AudioClip grenadeSFX;
    
    //old code
    
    // Play grenade explosion animation and sound after 5 seconds
    //     Invoke("PlayExplosion", 5f);
    //     // Disable the grenade collider
    //     other.gameObject.SetActive(false);
    //     Debug.Log("Collision with grenade");
    
    public void PlayExplosion()
    {
        // Check if the grenade animation and explosion sound are assigned
        if (grenadeAnimation != null && grenadeSFX != null)
        {
            // Play grenade explosion animation
            grenadeAnimation.Play();

            EnvironmentalSoundController.Instance.PlaySoundAtLocation(grenadeSFX, 1.25f, transform.position);
        }
        else
        {
            Debug.LogWarning("Grenade animation or explosion sound not assigned.");
        }
    }
}
