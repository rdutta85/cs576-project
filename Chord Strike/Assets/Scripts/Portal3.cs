using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Portal3 : MonoBehaviour
{
    public GameObject player; // Reference to the player
    //public GameObject gameCompletionCanvas; // Reference to the Game Completion UI (optional for now)

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the portal is the player
        if (other.gameObject == player)
        {
            Debug.Log("Player reached the portal!");
            TriggerGameCompletion();
        }
    }

    void TriggerGameCompletion()
    {
        // Logic for triggering game completion (placeholder for now)
        Debug.Log("Game Completed!");
        SceneManager.LoadScene("GameCompletion3");
        
        
    }
}