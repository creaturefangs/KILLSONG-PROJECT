using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public List<AudioClip> musicClips;  // List of audio clips to play
    public AudioSource audioSource;     // The audio source to play the clips through
    public float waitTimeInMinutes = 5f; // Time to wait between clips in minutes

    void Start()
    {
        if (musicClips.Count == 0)
        {
            Debug.LogWarning("No music clips assigned to the MusicManager.");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogWarning("No audio source assigned to the MusicManager.");
            return;
        }

        StartCoroutine(PlayMusic());
    }

    IEnumerator PlayMusic()
    {
        while (true)
        {
            // Choose a random clip from the list
            AudioClip randomClip = musicClips[Random.Range(0, musicClips.Count)];

            // Play the clip
            audioSource.clip = randomClip;
            audioSource.Play();

            // Wait for the clip to finish playing plus the wait time
            yield return new WaitForSeconds(randomClip.length + waitTimeInMinutes * 60);
        }
    }
}
