using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public int CurrentLevel { get; private set; } = 1;
    public TextMeshProUGUI levelText;
    private static int awakeCount = 0;



    private void Awake()
    {
        awakeCount++; // Increment the counter each time Awake is called

        // Check if this is the 3rd Awake call
        if (awakeCount % 3 == 0)
        {
            Debug.Log("Awake has been called 3 times.");
            AdsManager.Instance.ShowAd();

        }

        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            // Load the current level from PlayerPrefs
            CurrentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
            InitializeLevel(CurrentLevel);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator AssignLevelTextWithDelay()
    {
        // Wait for the scene to stabilize
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame(); // Wait an additional frame to ensure all objects are ready

        // Now try to find the levelText with extra validation
        var levelTextObj = GameObject.FindWithTag("LevelTextTag");
        if (levelTextObj)
        {
            levelText = levelTextObj.GetComponent<TextMeshProUGUI>();
            if (levelText)
            {
                UpdateLevelText();
            }
            else
            {
                Debug.LogError("The LevelText GameObject does not have a TextMeshProUGUI component.");
            }
        }
        else
        {
            Debug.LogError("LevelText GameObject with the 'LevelTextTag' tag was not found in the scene.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Only run the coroutine if we're in a scene that should have levelText
        if (scene.name == "SampleScene") // Replace with the actual name of your scene
        {
            StartCoroutine(AssignLevelTextWithDelay());
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to prevent memory leaks
    }
    public void InitializeLevel(int level)
    {
        CurrentLevel = level;
        UpdateLevelText(); // Updates the level display text

        // Save the current level
        PlayerPrefs.SetInt("CurrentLevel", CurrentLevel);
        PlayerPrefs.Save();
    }

    public void StartNextLevel()
    {
        CurrentLevel++;
        UpdateLevelText(); // Update the text UI
        GridPlacerWithBoxes.Instance.SetupNewLevel(CurrentLevel);

        // Save the current level
        PlayerPrefs.SetInt("CurrentLevel", CurrentLevel);
        PlayerPrefs.Save();
    }

    public void MoveToNextLevel()
    {
        // Logic to transition to the next level
        GameManager.Instance.StartNextLevel();
    }
    private void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = $"Level {CurrentLevel}";
        }
        else
        {
            Debug.LogError("LevelText is not assigned in the GameManager.");
        }
    }

}
