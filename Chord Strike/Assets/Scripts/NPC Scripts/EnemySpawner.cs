// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawner")]
    public bool startSpawning;
    private JunkochanControl junko;

    /*
     Tasks for R Dutta: 
     Create an enemy spawn controller that spawns enemies depending on the position of the player.
     Spawn 3 enemies at the same time within a 20 
    */

    public GameObject raptorPrefab;         // prefab for the raptor enemy
    public GameObject rhinoPrefab;         // prefab for the rhino enemy

    private string enemyTag = "Enemy";     // tag for the enemies
    private int numEnemies = 3;            // number of enemies expected in the scene 
    private float spawnRadius = 10.0f;     // radius of the circle in which the enemies are spawned
    private float spawnHeight = 5.0f;     // height of the enemy above the ground (released from the sky)
    private Bounds terrainBounds;          // bounds of the terrain

    private Vector3 RandomEnemyPos(int counter = 0)
    {
        float spawnX = Random.Range(-spawnRadius, spawnRadius);
        float spawnZ = Mathf.Sqrt(spawnRadius * spawnRadius - spawnX * spawnX) * Mathf.Sign(Random.Range(-1.0f, 1.0f));
        float spawnY = junko.transform.position.y + spawnHeight;

        spawnX += junko.transform.position.x;
        spawnZ += junko.transform.position.z;

        Vector3 pos = new Vector3(spawnX, spawnY, spawnZ);

        // confirm if the spawn position is within the terrain bounds
        if (terrainBounds.Contains(pos)) return pos;
        else
        {
            counter++;
            if (counter > 10000) return pos;
            else return RandomEnemyPos(counter);
        }

    }

    private Quaternion EnemyOrientation(Vector3 spawnPos)
    {
        Vector3 lookAtPos = junko.transform.position;
        lookAtPos.y = 0.0f;
        return Quaternion.LookRotation(lookAtPos - spawnPos);
    }

    private GameObject CreateDummyEnemy(Vector3 spawnPos, Quaternion spawnRot)
    {
        GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Cube);

        enemy.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        enemy.transform.position = spawnPos;
        enemy.transform.rotation = spawnRot;
        enemy.GetComponent<Renderer>().material.color = Color.red;
        enemy.AddComponent<Rigidbody>();
        enemy.GetComponent<Rigidbody>().useGravity = true;
        enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        return enemy;
    }


    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // randomly select an enemy type
            GameObject currentEnemy;

            int enemyType = 0; //UnityEngine.Random.Range(0, 2);
            switch (enemyType)
            {
                case 0:
                    currentEnemy = raptorPrefab;
                    break;
                case 1:
                    currentEnemy = rhinoPrefab;
                    break;

                default:
                    currentEnemy = raptorPrefab;
                    break;

            }


            Vector3 spawnPos = RandomEnemyPos();
            Quaternion spawnRot = EnemyOrientation(spawnPos);

            GameObject enemy = Instantiate(currentEnemy, spawnPos, spawnRot);
            enemy.name = enemyTag;
            enemy.gameObject.tag = enemyTag;

            enemy.AddComponent<BoxCollider>();
            enemy.AddComponent<Rigidbody>();
            enemy.GetComponent<Rigidbody>().useGravity = true;

            // Add script to the enemy
            switch (enemyType)
            {
                case 0:
                    enemy.AddComponent<Raptor>();
                    break;
                case 1:
                    enemy.AddComponent<Rhino>();
                    break;

                default:
                    enemy.AddComponent<Raptor>();
                    break;


            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();

        terrainBounds = GameObject.Find("Terrain").GetComponent<TerrainCollider>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning)
        {
            StartCoroutine("SpawnEnemy");
        }
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            // find number of enemies in the scene and spawn more if necessary
            // name the enemies as enemyTag
            if (GameObject.Find(enemyTag) == null)
            {
                SpawnEnemies(numEnemies);
            }
            else
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
                if (enemies.Length < numEnemies)
                {
                    SpawnEnemies(numEnemies - enemies.Length);
                }
            }
        }
    }
}
