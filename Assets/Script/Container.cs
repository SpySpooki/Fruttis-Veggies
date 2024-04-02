using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Container : MonoBehaviour, IDropHandler
{
    public const int MaxChildren = 3; // Maximum children a container can have
    public ParticleSystem matchParticlesPrefab;
    public Button nextLevelButtonPrefab;
    private bool levelEnded = false;
    [HideInInspector]public Image containerImage; // Reference to the container's Image component
    private Color currentColor; // Current color of the container
    
    private static int endLevelCallCount = 0;
    private void Awake()
    {
        containerImage = GetComponent<Image>(); // Assign the Image component
    }
    public void Update()
    {
        CheckForEndLevelCondition();
    }
    public void SetRandomColorAndMatchParticles()
    {
        containerImage.color = currentColor;

    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount < MaxChildren)
        {
            MusicController.Instance.PlaySound(MusicController.Instance.dropClip);
            GameObject droppedObject = eventData.pointerDrag;
            DraggableItem draggableItem = droppedObject.GetComponent<DraggableItem>();
            if (draggableItem != null)
            {
                draggableItem.transform.SetParent(transform);
                draggableItem.isOverContainer = true; // Mark that it's successfully dropped over a container
                CheckForThreeOfAKindInThisBox();
                
            }
        }
    }
    public void ChangeSprite(Sprite newSprite)
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.sprite = newSprite;
        }
    }
    private void CheckForThreeOfAKindInThisBox()
    {
        Dictionary<string, int> tagCount = new Dictionary<string, int>();

        foreach (Transform itemTransform in transform)
        {
            string itemTag = itemTransform.tag;
            if (tagCount.ContainsKey(itemTag))
            {
                tagCount[itemTag]++;
            }
            else
            {
                tagCount.Add(itemTag, 1);
            }
        }

        // Check if any tag count has reached 3
        foreach (var kvp in tagCount)
        {
            if (kvp.Value == 3)
            {
                PlayMatchParticles(MakeDarker(containerImage.color)); // Match particles with a slightly darker shade
                MusicController.Instance.PlaySound(MusicController.Instance.MatchClip);
                // Schedule the container for deactivation after the particle effect
                StartCoroutine(DelayedDeactivation(0.7f));

                break; // Stop checking this box, as it's already scheduled for deactivation
            }
        }
    }

    // Coroutine for delayed deactivation
    private IEnumerator DelayedDeactivation(float delayTime)
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delayTime);

        // Deactivate the container GameObject to leave an empty space in the grid
        gameObject.SetActive(false);
    }

    private void CheckForEndLevelCondition()
    {
        RectTransform gridRectTransform = GridPlacerWithBoxes.Instance.gridRectTransform;
        // Check the number of active boxes in the parent container/grid
        int activeBoxCount = 0;
        foreach (Transform child in gridRectTransform) // Assuming gridRectTransform is the parent that holds all boxes
        {
            if (child.gameObject.activeSelf)
            {
                activeBoxCount++;
            }
        }

        // If there's only one box left, end the level
        if (activeBoxCount == 1)
        {
            EndLevel();
        }
    }


    private void EndLevel()
    {
        if (levelEnded) return;
        endLevelCallCount++;
        if (endLevelCallCount % 3 == 0)
        {
            Debug.Log("EndLevel has been called 3 times.");
            AdsManager.Instance.ShowAd();
        }
        MusicController.Instance.PlaySound(MusicController.Instance.endLevelClip);
        Canvas canvas = GameObject.FindWithTag("CanvasTag").GetComponent<Canvas>();

        GameObject nextLevelButtonInstance = Instantiate(nextLevelButtonPrefab.gameObject, canvas.transform);

        RectTransform btnRectTransform = nextLevelButtonInstance.GetComponent<RectTransform>();
        btnRectTransform.anchoredPosition = Vector2.zero;
        btnRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        btnRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        btnRectTransform.pivot = new Vector2(0.5f, 0.5f);

        nextLevelButtonInstance.SetActive(true);

        Button nextLevelButton = nextLevelButtonInstance.GetComponent<Button>();
        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(() => ButtonClicked(nextLevelButtonInstance));
        }
        levelEnded = true;
        Debug.Log("Level Ended - Next Level Button Instantiated on Canvas.");
    }

    private void ButtonClicked(GameObject buttonInstance)
    {
        GameManager.Instance.MoveToNextLevel();

        Destroy(buttonInstance);
    }



    public void PlayMatchParticles(Color particleColor)
    {
        if (matchParticlesPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, -2); // Set Z to -2
            ParticleSystem particlesInstance = Instantiate(matchParticlesPrefab, spawnPosition, Quaternion.identity);
            var mainModule = particlesInstance.main;
            mainModule.startColor = particleColor; // Set the darker color for the particles
            particlesInstance.Play();

            Destroy(particlesInstance.gameObject, particlesInstance.main.duration);
        }
    }

    public Color MakeDarker(Color color)
    {
        // Here we reduce the brightness by multiplying each color component by a factor less than 1
        float darkeningFactor = 0.5f; // Adjust this to make the color darker as desired
        return new Color(color.r * darkeningFactor, color.g * darkeningFactor, color.b * darkeningFactor);
    }



}
