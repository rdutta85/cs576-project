using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;

public class GameCompletion3 : MonoBehaviour
{
    public Button level1Button; // Button for transitioning to Level2
    public Button mainMenuButton; // Button for transitioning to the main menu

    void Start()
    {
        // Assign button functionality
        if (level1Button != null)
        {
            level1Button.onClick.AddListener(GoToLevel2);
        }
        else
        {
            Debug.LogError("Level1 Button is not assigned!");
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }
        else
        {
            Debug.LogError("Main Menu Button is not assigned!");
        }
    }

    void GoToLevel2()
    {
        // Load the Level2 scene
        SceneManager.LoadScene("Level2");
    }

    void GoToMainMenu()
    {
        // Load the Main Menu scene
        SceneManager.LoadScene("SampleScene");
    }
}
