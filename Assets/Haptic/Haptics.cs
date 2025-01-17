using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR;

public class Haptics : MonoBehaviour
{
    public GameObject boxesParent; // The parent GameObject holding all boxes
    public Transform handController; // Reference to the user's hand/controller position
    public float minPulseInterval = 1f; // Minimum time between pulses (in seconds) when misaligned
    public float maxPulseInterval = 0.1f; // Maximum frequency (shortest interval) when perfectly aligned
    public float alignmentThreshold = 0.95f; // Threshold for "close enough" alignment (cosine similarity)

    private bool isHapticsActive = false;
    private List<GameObject> boxes = new List<GameObject>();
    private InputDevice controllerDevice; // InputDevice for the VR controller
    private GameObject correctBox;
    private bool boxSelected = false;

    private float pulseTimer = 0f; // Timer for managing pulses
    private float currentPulseInterval = 1f; // Current interval between pulses


    void Start()
    {
        // Initialize the controller device
        controllerDevice = GetControllerDevice();

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
        if (isHapticsActive && controllerDevice.isValid)
        {
            // Find the correct box based on the isCorrectBox property
            foreach (var box in boxes)
            {
                var specialBox = box.GetComponent<specialBox>();
                if (specialBox != null && specialBox.specialBoxTrue) // Check if it's the correct box
                {
                    correctBox = box;
                    break; // Exit loop once the correct box is found
                }
            }

            if (correctBox != null && correctBox.GetComponent<specialBox>().specialBoxTrue)
            {
                // Calculate directional alignment
                Vector3 toBox = (correctBox.transform.position - handController.position).normalized; // Direction to the box
                Vector3 controllerDirection = handController.forward; // Forward direction of the controller

                float alignment = Vector3.Dot(toBox, controllerDirection); // Cosine similarity between directions

                // Map alignment to pulse interval (higher alignment = faster pulse)
                currentPulseInterval = Mathf.Lerp(minPulseInterval, maxPulseInterval, Mathf.Clamp01(alignment));

                // Handle pulsating vibration
                pulseTimer += Time.deltaTime;
                if (pulseTimer >= currentPulseInterval)
                {
                    pulseTimer = 0f; // Reset timer
                    TriggerHaptics(1f); // Trigger a short pulse
                }
            }
            else
            {
                Debug.LogWarning("No correct box found with isCorrectBox set to true.");

            }
        }
    }

    public void EnableHaptics()
    {
        isHapticsActive = true;
    }

    public void DisableHaptics()
    {
        isHapticsActive = false;
        StopHaptics();
    }

    public void ToggleHaptics()
    {
        if (isHapticsActive)
        {
            DisableHaptics();
            if (EventLogger.Instance != null)
            {
                EventLogger.Instance.LogEvent($"Haptics Off");
            }
        }
        else
        {
            EnableHaptics();
            if (EventLogger.Instance != null)
            {
                EventLogger.Instance.LogEvent($"Haptics On");
            }
        }
    }


    private void TriggerHaptics(float intensity)
    {
        if (controllerDevice.isValid)
        {
            // Trigger haptic impulse
            controllerDevice.SendHapticImpulse(0, intensity, 0.1f); // Channel 0, Intensity, Duration
        }
    }

    private void StopHaptics()
    {
        if (controllerDevice.isValid)
        {
            // Stop haptic feedback by sending zero intensity
            controllerDevice.SendHapticImpulse(0, 0, 0);
        }
    }

    private InputDevice GetControllerDevice()
    {
        // Get the right-hand controller by default
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);

        if (devices.Count > 0)
        {
            return devices[0];
        }

        Debug.LogWarning("No right-hand controller found. Falling back to left hand.");
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);

        return devices.Count > 0 ? devices[0] : default;
    }
}

