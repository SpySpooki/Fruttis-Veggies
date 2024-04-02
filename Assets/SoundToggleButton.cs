using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for scene management

public class SoundToggleButton : MonoBehaviour
{
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public Image iconImage;

    private void Awake()
    {
        // Optionally, ensure this button is also persistent across scenes
        //DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
        UpdateButtonIconBasedOnCurrentSettings();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Clean up the event subscription
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateButtonIconBasedOnCurrentSettings(); // Update button state when a new scene is loaded
    }

    public void ToggleSound()
    {
        MusicController.Instance.ToggleMusic();
        MusicController.Instance.ToggleSoundEffects();
        // Assume ToggleSoundEffects is handled within ToggleMusic or similarly
        UpdateButtonIconBasedOnCurrentSettings();
    }

    private void UpdateButtonIconBasedOnCurrentSettings()
    {
        if (MusicController.Instance)
        {
            bool isAudioEnabled = MusicController.Instance.IsMusicOn(); // Simplified for clarity
            iconImage.sprite = isAudioEnabled ? soundOnSprite : soundOffSprite;
        }
    }
}
