using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver2 : MonoBehaviour
{
    public GameObject gameOverCanvas; // Reference to the Game Over Canvas
    public GameObject previousCanvas;
    public JunkochanControl playerControl; // Reference to the player script

    public Button retryButton; // Reference to the Retry button
    public Button mainMenuButton; // Reference to the Main Menu button
    public Button LoadLevel1;     //Reference to  the level1

    void Start()
    {
        // Assign button functionality programmatically
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(RetryLevel);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }
        if (LoadLevel1 != null)
        {
            LoadLevel1.onClick.AddListener(loadlevel1);
        }
        // Ensure the Game Over Canvas is hidden initially
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (playerControl.Health <= 0 && !gameOverCanvas.activeSelf)
        {
            ShowGameOverMenu();
        }
    }

    public void ShowGameOverMenu()
    {
        if (previousCanvas != null)
        {
            previousCanvas.SetActive(false); // Deactivate the previous canvas
        }
        gameOverCanvas.SetActive(true); // Activate the Game Over Canvas
        Time.timeScale = 0f; // Pause the game
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current level
    }

    public void loadlevel1()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene("Level1");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }
}


