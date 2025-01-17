using UnityEngine;
using System.Collections.Generic; // To use List (for storing boxes)
using UnityEngine.InputSystem; // If using the new Input System

public class NewEmptyCSharpScript : MonoBehaviour
{
    public Material newMaterial;    // The material to change to
    public Material defaultMaterial; // Default material to reset
    public GameObject boxesParent; // The parent GameObject holding all boxes

    private List<GameObject> boxes = new List<GameObject>(); // List to store the child boxes
    private GameObject lastChangedBox = null; // To store the last box that had its material changed

    void Start()
    {
        // Populate the boxes list from the children of the parent GameObject
        if (boxesParent != null)
        {
            foreach (Transform child in boxesParent.transform)
            {
                boxes.Add(child.gameObject);
            }

            Debug.Log($"Found {boxes.Count} boxes under {boxesParent.name}");
        }
        else
        {
            Debug.LogError("boxesParent is not assigned. Please assign the parent GameObject in the Inspector.");
        }
    }

    void Update()
    {
        // Using Input System (optional)
        if (Keyboard.current.spaceKey.wasPressedThisFrame) // When Spacebar is pressed
        {
            Debug.Log("Spacebar pressed");
            RandomBox();
        }
    }

    public void RandomBox()
    {
        // Ensure the list of boxes is not empty
        if (boxes.Count > 0 && newMaterial != null)
        {
            Debug.Log("Has boxes");

            // Choose a random index from the list of boxes
            int randomIndex = Random.Range(0, boxes.Count);

            specialBox boolComponent = boxes[randomIndex].GetComponent<specialBox>();

            // Apply the new material to the selected box
            if (boolComponent != null)
            {
                boolComponent.specialBoxTrue = true;

                AudioSource boxAudio = boxes[randomIndex].GetComponent<AudioSource>();
                if (boxAudio != null)
                {
                    boxAudio.volume = 1f;
                }

                Debug.Log($"Selected box: {boxes[randomIndex].name}");
                if (EventLogger.Instance != null)
                {
                    EventLogger.Instance.LogEvent($"Sound1");
                    EventLogger.Instance.LogEvent($"BoxSelected: {boxes[randomIndex].name}");
                }
                else
                {
                    Debug.LogWarning("LogScript instance not found!");
                }
            }
            else
            {
                Debug.LogWarning($"The box {boxes[randomIndex].name} does not have a specialBox component.");
            }
        }
        else
        {
            Debug.LogError("No boxes available or newMaterial is not assigned.");
        }
    }
}
