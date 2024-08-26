using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnvironmentalSoundController : MonoBehaviour
{
    private static EnvironmentalSoundController _instance;
    private AudioSource _audioSource;

    public static EnvironmentalSoundController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnvironmentalSoundController>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("EnvironmentalSoundController");
                    _instance = obj.AddComponent<EnvironmentalSoundController>();
                }
                DontDestroyOnLoad(_instance.gameObject);
                _instance._audioSource = _instance.GetComponent<AudioSource>();
                if (_instance._audioSource == null)
                {
                    _instance._audioSource = _instance.gameObject.AddComponent<AudioSource>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private bool IsSourcePlaying() => _audioSource.isPlaying;

    /// <summary>
    /// Play a specified sound effect globally(2D) at a specified volume level
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volumeScale"></param>
    public void PlaySound2D(AudioClip clip, float volumeScale)
    {
        _audioSource.spatialBlend = 0f;
        
        if(!IsSourcePlaying())
            _audioSource.PlayOneShot(clip, volumeScale);
    }

    /// <summary>
    /// Play a specified sound effect locally(3D) at a specified volume level at a specified location
    /// To play a sound at an objects location, call PlaySoundAtLocation and for last param, use transform.position
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volumeScale"></param>
    /// <param name="position"></param>
    public void PlaySoundAtLocation(AudioClip clip, float volumeScale, Vector3 position)
    {
        transform.position = position;

        _audioSource.spatialBlend = 1.0f;
        if (!IsSourcePlaying())
        {
            _audioSource.PlayOneShot(clip,volumeScale);
        }
    }
}
