using UnityEngine;

public class DirectionalSoundController : MonoBehaviour
{
    public Transform vrCamera; // Drag the VR camera here (from your XR Rig).
    public GameObject boxesParent; // The parent GameObject that holds all the boxes.
    public AudioSource audioSource; // Drag the AudioSource GameObject here.

    private GameObject[] boxes; // Array of target points (now automatically populated).
    private Transform selectedTarget; // The currently selected target.

    public float basePitch = 1.0f; // Starting pitch of the sound.
    public float pitchMultiplier = 4.0f; // How much the pitch increases when looking directly at the target.

    void Start()
    {
        // Ensure there are targets and an audio source
        if (boxesParent == null || audioSource == null || vrCamera == null)
        {
            Debug.LogError("Setup is incomplete. Ensure all references are assigned.");
            return;
        }

        // Populate the boxes array with the child objects of the boxesParent
        boxes = new GameObject[boxesParent.transform.childCount];
        for (int i = 0; i < boxesParent.transform.childCount; i++)
        {
            boxes[i] = boxesParent.transform.GetChild(i).gameObject;
        }

        // Start with the sound turned off
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    public void StartGame()
    {
        // Randomly select one of the boxes
        int randomIndex = Random.Range(0, boxes.Length);
        selectedTarget = boxes[randomIndex].transform; // Get the Transform of the selected box

        Debug.Log("Target selected: " + selectedTarget.name);

        specialBox boolComponent = boxes[randomIndex].GetComponent<specialBox>();
        boolComponent.specialBoxTrue = true;

        if (EventLogger.Instance != null)
        {
            EventLogger.Instance.LogEvent($"Sound2");
            EventLogger.Instance.LogEvent($"BoxSelected: {boxes[randomIndex].name}");
        }
        else
        {
            Debug.LogWarning("LogScript instance not found!");
        }


        // Start the sound
        audioSource.volume = 1;
    }

    void Update()
    {
        if (selectedTarget != null && audioSource.isPlaying)
        {
            // Calculate the angle between the camera's forward direction and the direction to the target
            Vector3 directionToTarget = (selectedTarget.position - vrCamera.position).normalized;
            float angle = Vector3.Angle(vrCamera.forward, directionToTarget);

            // Adjust pitch based on the angle (smaller angle = closer to looking directly at the target)
            float normalizedAngle = Mathf.Clamp01(1 - (angle / 90.0f)); // Normalize to [0, 1]
            audioSource.pitch = basePitch + (normalizedAngle * pitchMultiplier);
        }
    }
}
