using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneChangerVR : MonoBehaviour
{
    // The name of the scene to load
    public string sceneToLoad;

    // This method is called when the interactable is selected
    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        LoadScene();
    }

    // Load the specified scene
    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Scene name not set!");
        }
    }
}