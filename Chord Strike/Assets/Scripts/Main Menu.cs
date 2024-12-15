using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;
    public Button tutorial;
    // Start is called before the first frame update
    void Start()
    {
        button1.onClick.AddListener(delegate { ProcessButtonInput(1); });
        button2.onClick.AddListener(delegate { ProcessButtonInput(2); });
        button3.onClick.AddListener(delegate { ProcessButtonInput(3); });
        tutorial.onClick.AddListener(delegate { ProcessButtonInput(4); });
    }

    void ProcessButtonInput(int id)
    {
        if (id == 1)
        {
            Debug.Log("Button 1 clicked");
            SceneManager.LoadScene("Scenes/Level1");
        }
        if (id == 2)
        {
            Debug.Log("Button 2 clicked");
            SceneManager.LoadScene("Scenes/Level2");
        }
        if (id == 3)
        {
            Debug.Log("Button 3 clicked");
            SceneManager.LoadScene("Scenes/Level3");
        }
        if (id == 4)
        {
            Debug.Log("Tutorial clicked");
            SceneManager.LoadScene("Scenes/Level0");
        }
    }
}
