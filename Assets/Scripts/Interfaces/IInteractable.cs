using UnityEngine;

public interface IInteractable
{
    void Interact();
    void OnPickup();
    void PlaySoundOnInteraction(AudioSource source, AudioClip sound);
}
