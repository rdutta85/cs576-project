using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public JunkochanControl junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();

    /*
     Tasks for R Dutta: 
     Create an enemy spawn controller that spawns enemies depending on the position of the player.
     Spawn 3 enemies at the same time within a 20 
    */

    public GameObject enemyPrefab;         // prefab for the enemy

    private string enemyTag = "Enemy";     // tag for the enemies
    private int numEnemies = 3;            // number of enemies expected in the scene 
    private float spawnRadius = 20.0f;     // radius of the circle in which the enemies are spawned
    private float spawnHeight = 10.0f;     // height of the enemy above the ground (released from the sky)

    private Vector3 RandomEnemyPos()
    {
        float spawnX = Random.Range(-spawnRadius, spawnRadius);
        float spawnZ = Mathf.Sqrt(spawnRadius * spawnRadius - spawnX * spawnX) * Mathf.Sign(Random.Range(-1.0f, 1.0f));
        return new Vector3(spawnX, spawnHeight, spawnZ);
    }

    private Vector3 EnemyOrientation(Vector3 spawnPos)
    {
        Vector3 lookAtPos = junko.transform.position;
        lookAtPos.y = 0.0f;
        return Quaternion.LookRotation(lookAtPos - spawnPos);
    }

    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = RandomEnemyPos();
            Vector3 spawnRot = EnemyOrientation(spawnPos);

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, spawnRot);
            enemy.name = enemyTag;

            enemy.AddComponent<Enemy>();
            enemy.GetComponent<Rigidbody>().mass = 1000.0f;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemies(numEnemies);
    }

    // Update is called once per frame
    void Update()
    {
        // find number of enemies in the scene and spawn more if necessary
        // name the enemies as enemyTag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        if (enemies.Length < numEnemies)
        {
            SpawnEnemies(numEnemies - enemies.Length);
        }
    }
}
