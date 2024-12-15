using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    string mainMenuScene = "MainMenu";
    void Start()
    {
        // Unload all scenes except the SampleScene
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != mainMenuScene)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        // Load the Main Menu scene
        if (SceneManager.GetActiveScene().name != mainMenuScene)
        {
            SceneManager.LoadScene(mainMenuScene, LoadSceneMode.Single);
        }
    }

    public void LoadLevel1()
    {

        SceneManager.LoadScene("Level1");
    }
    public void LoadLevel2()
    {

        SceneManager.LoadScene("Level2");
    }

    public void LoadLevel3()
    {

        SceneManager.LoadScene("Level3");
    }
}
