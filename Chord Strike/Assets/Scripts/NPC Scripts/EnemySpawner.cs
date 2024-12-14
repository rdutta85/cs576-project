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

    public GameObject enemyPrefab;         // prefab for the enemy

    private string enemyTag = "Enemy";     // tag for the enemies
    private int numEnemies = 3;            // number of enemies expected in the scene 
    private float spawnRadius = 10.0f;     // radius of the circle in which the enemies are spawned
    private float spawnHeight = 5.0f;     // height of the enemy above the ground (released from the sky)

    private Vector3 RandomEnemyPos()
    {
        float spawnX = Random.Range(-spawnRadius, spawnRadius);
        float spawnZ = Mathf.Sqrt(spawnRadius * spawnRadius - spawnX * spawnX) * Mathf.Sign(Random.Range(-1.0f, 1.0f));
        return new Vector3(
            junko.transform.position.x + spawnX,
            junko.transform.position.y + spawnHeight,
            junko.transform.position.z + spawnZ);
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
            Vector3 spawnPos = RandomEnemyPos();
            Quaternion spawnRot = EnemyOrientation(spawnPos);

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, spawnRot);
            enemy.name = enemyTag;
            enemy.gameObject.tag = enemyTag;

        }
    }


    // Start is called before the first frame update
    void Start()
    {
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();
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
