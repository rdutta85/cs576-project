using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public GameObject player; // Reference to the player
    protected string gameOverScene; // Name of the Game Over scene
    protected int enemyThreshold;

    [Header("Activate Portal")]
    public bool activated = false;
    protected EnemySpawner enemySpawner; // Reference to the Enemy Spawner script

    protected ParticleSystem ps;

    protected void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();

        // Get Enemy Spawner script from the Terrain object
        enemySpawner = GameObject.Find("Terrain").GetComponent<EnemySpawner>();

        // add an ienumerator to check enemy counter in enemy spawner to activate portal
        StartCoroutine(CheckEnemyCounter());
    }

    IEnumerator CheckEnemyCounter()
    {
        while (true)
        {
            // sleep for 1 second
            yield return new WaitForSeconds(0.5f);

            // Check if the enemy counter is zero
            if (enemySpawner.spawnCounter >= enemyThreshold || activated)
            {
                // Activate the portal
                activated = true;

                // change the color of the particle system to blue
                var main = ps.main;
                main.startColor = Color.blue;


                Debug.Log("Portal activated!");
            }
            yield return new WaitForSeconds(1f);
        }
    }

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
        if (!activated)
        {
            Debug.Log("Portal not activated yet!");
            return;
        }

        // Logic for triggering game completion (placeholder for now)
        Debug.Log("Game Completed!");
        SceneManager.LoadScene("GameCompletion1");

        /***if (gameCompletionCanvas != null)
        {
            gameCompletionCanvas.SetActive(true); // Show the Game Completion UI
            Time.timeScale = 0f; // Pause the game
        }***/
    }



}