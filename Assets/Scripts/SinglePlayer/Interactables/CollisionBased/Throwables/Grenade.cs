using UnityEngine;

public class Grenade : Throwable
{
    [SerializeField] private Animation grenadeAnimation;
    [SerializeField] private AudioClip grenadeSFX;
    
    public void PlayExplosion()
    {
        // Play grenade explosion animation
        //grenadeAnimation.Play();
        EnvironmentalSoundController.Instance.PlaySoundAtLocation(grenadeSFX, 1.25f, transform.position);
    }
}
