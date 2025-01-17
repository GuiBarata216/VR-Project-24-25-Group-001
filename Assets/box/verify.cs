using UnityEngine;
using System.Collections;
using UnityEngine.VFX;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class verify : MonoBehaviour
{

    private GameObject targetBox ; // The box to be checked
    public Material correctSelectionBox;  // The mesh to change to for 1.5 seconds
    public Material defaultBox;  // The mesh to change to after 1.5 seconds
    public Material wrongBox;  // The mesh to change to after 1.5 seconds


    private Renderer boxRenderer;  // Renderer to change mesh

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetBox = gameObject;
    }

    // Update is called once per frame
    public void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame) // When Spacebar is pressed
        {
            Debug.Log("enterKey");

            VerifyAndChangeMesh();
        }
    }


    public void VerifyAndChangeMesh()
    {
        // Get the Renderer component of the targetBox
        boxRenderer = targetBox.GetComponent<Renderer>();
        specialBox boolComponent = targetBox.GetComponent<specialBox>();

        if (boolComponent != null)
        {
            // Compare the material of the targetBox with specialBox
            if (boolComponent.specialBoxTrue)
            {
                // The box has the same material as the specialBox
                Debug.Log("Correct box selected!");

                // You can change the material to the correct selection box
                boxRenderer.material = correctSelectionBox;
                boolComponent.specialBoxTrue = false;
                // Start the coroutine to handle the change back to default after 1.5 seconds
                StartCoroutine(HandleMeshAndSound());
                
                if (EventLogger.Instance != null)
                {
                    EventLogger.Instance.LogEvent($"Correct: {targetBox.name}");
                }
                else
                {
                    Debug.LogWarning("LogScript instance not found!");
                }


            }
            else
            {
                boxRenderer.material = wrongBox;
                StartCoroutine(HandleMeshAndSound());

                if (EventLogger.Instance != null)
                {
                    EventLogger.Instance.LogEvent($"Wrong: {targetBox.name}");
                }
                else
                {
                    Debug.LogWarning("LogScript instance not found!");
                }

            }
        }
        else
        {
            Debug.LogError("Target box does not have a Renderer component.");
        }
    }

    private IEnumerator HandleMeshAndSound()
    {
        AudioSource boxAudio = targetBox.GetComponent<AudioSource>();

        // Disable the sound (set the volume to 0)
        if (boxAudio != null)
        {
            boxAudio.volume = 0f; // Mute the sound
            Debug.Log("Sound disabled.");
        }
        // Wait for 1.5 seconds
        yield return new WaitForSeconds(1.5f);

        // Change the mesh back to W
        if (boxRenderer != null)
        {
            boxRenderer.material = defaultBox;
            Debug.Log("Mesh changed back to W.");
        }
    }



}
