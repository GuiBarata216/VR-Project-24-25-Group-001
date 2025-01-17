using UnityEngine;

public class FasterSoundControl : MonoBehaviour
{
    public Transform vrCamera; // Drag the VR camera here (from your XR Rig).
    public GameObject boxesParent; // The parent GameObject that holds all the boxes.
    public AudioSource audioSource; // Drag the AudioSource GameObject here.
    public AudioClip beepSound; // Assign the beep sound clip here.

    private Transform[] boxes; // Array of target boxes (now using Transform).
    private Transform selectedTarget; // The currently selected target.
    private float nextBeepTime = 0f; // Tracks when the next beep should play.

    private float minInterval = 0.1f; // Interval between beeps for the correct box.
    private float maxInterval = 1.0f; // Interval between beeps for the wrong box.
    private float angleThreshold = 45f; // Angle threshold to determine if the user is looking at the box.

    void Start()
    {
        // Ensure there are targets and an audio source
        if (boxesParent == null || audioSource == null || vrCamera == null)
        {
            Debug.LogError("Setup is incomplete. Ensure all references are assigned.");
            return;
        }

        // Ensure that the audio source has an audio clip assigned
        if (beepSound == null)
        {
            Debug.LogError("No beep sound clip assigned.");
            return;
        }

        // Populate the boxes array with the child transforms of the boxesParent
        boxes = new Transform[boxesParent.transform.childCount];
        for (int i = 0; i < boxesParent.transform.childCount; i++)
        {
            boxes[i] = boxesParent.transform.GetChild(i); // Get each child Transform
        }

        // AudioSource setup
        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    public void StartGame()
    {
        // Randomly select one of the boxes
        int randomIndex = Random.Range(0, boxes.Length);
        selectedTarget = boxes[randomIndex]; // Get the selected target Transform

        Debug.Log("Target selected: " + selectedTarget.name);

        // Make the selected box a "special box"
        specialBox boolComponent = selectedTarget.GetComponent<specialBox>();

        audioSource.volume = 1;

        if (EventLogger.Instance != null)
        {
            EventLogger.Instance.LogEvent($"Sound3");
            EventLogger.Instance.LogEvent($"BoxSelected: {selectedTarget.name}");

        }
        else
        {
            Debug.LogWarning("LogScript instance not found!");
        }


        if (boolComponent != null)
        {
            boolComponent.specialBoxTrue = true;
        }
        else
        {
            Debug.LogWarning("No specialBox component found on " + selectedTarget.name);
        }
    }

    void Update()
    {
        if (selectedTarget != null)
        {
            // Calculate the angle between the camera's forward direction and the direction to the target
            Vector3 directionToTarget = (selectedTarget.position - vrCamera.position).normalized;
            float angleToTarget = Vector3.Angle(vrCamera.forward.normalized, directionToTarget);

            // Normalize angle to range [0, 1]
            float normalizedAngle = Mathf.Clamp01(angleToTarget / angleThreshold);

            // Interpolate interval based on normalized angle
            float currentInterval = Mathf.Lerp(minInterval, maxInterval, normalizedAngle);

            Debug.Log($"Angle to target: {angleToTarget}, Normalized: {normalizedAngle}, Interval: {currentInterval}");

            if (Time.time >= nextBeepTime)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(beepSound);
                nextBeepTime = Time.time + currentInterval;
            }
        }
    }
}
