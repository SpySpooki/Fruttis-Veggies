using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance;

    private AudioSource audioSource;
    private bool isMusicOn = true;
    private bool isSoundEffectsOn = true;
    private float soundEffectsVolume = 0.1f;
    [SerializeField] public AudioClip endLevelClip;
    [SerializeField] public AudioClip dragClip;
    [SerializeField] public AudioClip dropClip;
    [SerializeField] public AudioClip MatchClip;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlaySound(AudioClip clip)
    {
        if (isSoundEffectsOn)
        {
            // Assuming you have an AudioSource component attached to GameManager or elsewhere
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, soundEffectsVolume);
        }
    }
    public void ToggleSoundEffects()
    {
        isSoundEffectsOn = !isSoundEffectsOn;
    }

    public bool IsSoundEffectsOn()
    {
        return isSoundEffectsOn;
    }
    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        audioSource.mute = !isMusicOn;
    }

    // You might want a method to check the current state of the music
    public bool IsMusicOn()
    {
        return isMusicOn;
    }
}
