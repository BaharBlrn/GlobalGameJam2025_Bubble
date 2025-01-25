using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // This is needed for UI Button
using UnityEngine.SceneManagement;  // Required for SceneManager


public class SceneChanger : MonoBehaviour
{
    // Assign your button in the Unity Inspector
    public Button Button_NewLevels;

    void Start()
    {
        // Add a listener to your button so that it triggers a scene change on click
        if (Button_NewLevels != null)
        {
            Button_NewLevels.onClick.AddListener(ChangeScene);
        }
    }

    // Method to change the scene
    public void ChangeScene()
    {
        // Specify the scene name or build index you want to load
        // Example using scene name:
        SceneManager.LoadScene("NewLevels");
        
        // Or, if you're using a build index (e.g., the second scene in your Build Settings):
        // SceneManager.LoadScene(1); // 0 is the first scene, 1 is the second, etc.
    }
}



