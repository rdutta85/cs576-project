using UnityEngine;
using UnityEngine.SceneManagement; 

public class LoadLevel : MonoBehaviour
{
    
    public void LoadLevel1()
    {
        
        SceneManager.LoadScene("Level1");
    }
    public void LoadLevel2()
    {

        SceneManager.LoadScene("Level2");
    }
}
