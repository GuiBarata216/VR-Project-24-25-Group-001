using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    private EventLogger logger;

    void Start()
    {
        // Find the logger in the scene
        logger = Object.FindFirstObjectByType<EventLogger>();
    }

    public void OnObjectGrabbed()
    {
        // Log the grab event
        logger.LogEvent("ObjectGrabbed");
    }

    public void OnObjectSelected()
    {
        // Log the select event
        logger.LogEvent("ObjectSelected");
    }
}
