using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GridPlacerWithBoxes : MonoBehaviour
{
    public GameObject boxPrefab; // The box prefab
    public List<GameObject> fruitAndVegetablePrefabs; // The list of item prefabs
    public static GridPlacerWithBoxes Instance { get; private set; }
    public RectTransform gridRectTransform; // The grid's RectTransform
    public Image backgroundImage; // Reference to the container's background image
    public List<Sprite> levelBackgroundSprites;

    private List<GameObject> placementList = new List<GameObject>();
    private Color levelColor; // Random color for the level

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetupNewLevel(GameManager.Instance.CurrentLevel);
    }

    void PopulatePlacementList(int numberOfBoxes)
    {
        placementList.Clear(); // Clear the list before populating it

        // Calculate the variety of items based on the number of boxes.
        // Each box will have one type of item, and since each box has 3 items, multiply by 3.
        int itemVariety = numberOfBoxes;
        int totalItemsNeeded = numberOfBoxes * 3;

        for (int i = 0; i < totalItemsNeeded; i++)
        {
            // Use modulo to loop through the item prefabs and only take as many types as itemVariety.
            GameObject itemPrefab = fruitAndVegetablePrefabs[i % itemVariety];
            placementList.Add(itemPrefab);
        }
    }

    public void SetupNewLevel(int level)
    {
        ClearOldLevel();
        ChangeBackgroundRandomly(); // Now changes background randomly
        int numberOfBoxes = CalculateBoxesForLevel(level);
        PopulatePlacementList(numberOfBoxes);
        ShuffleList(placementList);

        // Generate a random color for the level
        levelColor = GetRandomColor();

        PlaceItemsInGridWithBoxes(numberOfBoxes);
    }

    private void ClearOldLevel()
    {
        // Logic to clear the old boxes and items
        foreach (Transform child in gridRectTransform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ChangeBackgroundRandomly()
    {
        if (levelBackgroundSprites != null && levelBackgroundSprites.Count > 0)
        {
            int randomIndex = Random.Range(0, levelBackgroundSprites.Count);
            backgroundImage.sprite = levelBackgroundSprites[randomIndex];
        }
        else
        {
            Debug.LogError("Background sprite list is empty.");
        }
    }

    void ShuffleList(List<GameObject> list)
    {
        // Shuffle the list
        for (int i = 0; i < list.Count; i++)
        {
            var temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private int CalculateBoxesForLevel(int level)
    {
        // Generate a random number of steps between 0 and 5, inclusive
        int steps = Random.Range(0, 9);

        // Calculate the number of boxes as 12 + (steps * 3)
        int numberOfBoxes = 12 + (steps * 3);
        return numberOfBoxes;
        
        
    }

    void PlaceItemsInGridWithBoxes(int numberOfBoxes)
    {
        List<GameObject> allBoxes = new List<GameObject>(); // List to keep track of all the created boxes

        // Instantiate boxes and place items
        for (int boxIndex = 0; boxIndex < numberOfBoxes; boxIndex++)
        {
            // Instantiate a new box
            GameObject box = Instantiate(boxPrefab, gridRectTransform);
            allBoxes.Add(box);
            RectTransform boxRect = box.GetComponent<RectTransform>();
            boxRect.anchoredPosition = Vector2.zero; // Or any other specific position within its cell
            boxRect.localRotation = Quaternion.identity;
            boxRect.localScale = Vector3.one;

            // Set random color and match particles for the container
            Container container = box.GetComponent<Container>();
            if (container != null)
            {
                container.containerImage.color = levelColor; // Assign the level color to the container
            }
            else
            {
                Debug.LogError("Container component not found on the new box game object.");
            }

            // Place items in the box
            for (int itemIndex = 0; itemIndex < 3; itemIndex++) // Always 3 items per box
            {
                int placementIndex = (boxIndex * 3) + itemIndex;
                if (placementIndex < placementList.Count)
                {
                    Instantiate(placementList[placementIndex], box.transform);
                }
            }
        }

        // Delete a random item from a random box
        RemoveRandomItemFromBoxes(allBoxes);
    }

    void RemoveRandomItemFromBoxes(List<GameObject> boxes)
    {
        if (boxes.Count > 0)
        {
            int randomBoxIndex = Random.Range(0, boxes.Count);
            GameObject selectedBox = boxes[randomBoxIndex];

            // Make sure the selected box has at least one child before trying to remove one
            if (selectedBox.transform.childCount > 0)
            {
                int randomChildIndex = Random.Range(0, selectedBox.transform.childCount);
                Transform childToBeDeleted = selectedBox.transform.GetChild(randomChildIndex);
                Destroy(childToBeDeleted.gameObject); // Destroy the selected child
            }
        }
    }
    public Color GetRandomColor()
    {
        // Convert the 50-200 range to 0-1 by dividing by 255
        float minColorValue = 50f / 255f;
        float maxColorValue = 200f / 255f;

        // Generate random color components within the specified range
        float r = Random.Range(minColorValue, maxColorValue);
        float g = Random.Range(minColorValue, maxColorValue);
        float b = Random.Range(minColorValue, maxColorValue);

        return new Color(r, g, b);
    }

    

}


