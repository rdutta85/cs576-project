// using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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

    public GameObject[] enemyPrefabs;         // prefab for the raptor enemy
    // public GameObject rhinoPrefab;         // prefab for the rhino enemy

    public string enemyTag = "Enemy";     // tag for the enemies
    private int numEnemies = 3;            // number of enemies expected in the scene 
    private float spawnRadius = 20.0f;     // radius of the circle in which the enemies are spawned
    private float spawnHeight = 10.0f;     // height of the enemy above the ground (released from the sky)
    private Bounds terrainBounds;          // bounds of the terrain
    public int spawnCounter = 0;
    // private NavMeshData navMeshData;


    private Vector3 RandomEnemyPos(int counter = 0)
    {
        float spawnX = Random.Range(-spawnRadius, spawnRadius);
        float spawnZ = Mathf.Sqrt(spawnRadius * spawnRadius - spawnX * spawnX) * Mathf.Sign(Random.Range(-1.0f, 1.0f));
        float spawnY = junko.transform.position.y + spawnHeight;

        spawnX += junko.transform.position.x;
        spawnZ += junko.transform.position.z;

        Vector3 pos = new Vector3(spawnX, spawnY, spawnZ);
        // Vector3 pos;

        // confirm if the spawn position is inside the navmesh surface
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 2 * spawnRadius, NavMesh.AllAreas))
        {
            pos = hit.position;
            // pos.y = spawnY;
            return pos;
        }
        else
        {
            counter++;
            if (counter > 10000) return new Vector3(0, spawnHeight, 0);
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
        int invalidSpawn = 0;
        for (int i = 0; i < count; i++)
        {
            // randomly select an enemy type
            int rndIdx = Random.Range(0, enemyPrefabs.Length);
            GameObject currentEnemy = enemyPrefabs[rndIdx];

            Vector3 spawnPos = RandomEnemyPos();

            if (!isValidSpawnPos(spawnPos))
            {
                i--;
                invalidSpawn++;
                if (invalidSpawn > 10000) break;

                continue;
            }

            Quaternion spawnRot = EnemyOrientation(spawnPos);

            GameObject enemy = Instantiate(currentEnemy, spawnPos, spawnRot);
            enemy.name = enemyTag;
            enemy.gameObject.tag = enemyTag;

            spawnCounter++;
        }
    }

    private bool isValidSpawnPos(Vector3 spawnPos)
    {
        // check if the spawn position is already occupied by another enemy
        // assume a cube of size 2.0f x 2.0f x 2.0f as the enemy size

        Vector3 enemySize = new Vector3(2.0f, 2.0f, 2.0f);
        Collider[] hitColliders = Physics.OverlapBox(spawnPos, enemySize);
        bool problem = false;

        if (hitColliders.Length > 0)
        {
            foreach (Collider hit in hitColliders)
            {
                if (hit.gameObject.tag == enemyTag || hit.gameObject.layer == 8)
                {
                    problem = true;
                    break;
                }
            }
        }
        if (problem) return false;

        NavMeshPath path = new NavMeshPath();
        if (!NavMesh.CalculatePath(spawnPos, junko.transform.position, NavMesh.AllAreas, path))
        {
            problem = true;
        }
        else
        {
            if (path.status != NavMeshPathStatus.PathComplete)
            {
                problem = true;
            }
        }
        if (!problem)
        {
            // draw a sline from the spawn position to the player position denoting the path
            // using path corners
            Vector3[] corners = path.corners;
            for (int i = 0; i < corners.Length - 1; i++)
            {
                Debug.DrawLine(corners[i], corners[i + 1], Color.red);
            }
        }

        return !problem;
    }

    // Start is called before the first frame update
    void Start()
    {
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();

        if (SceneManager.GetActiveScene().name == "Level2")
        {
            terrainBounds = GameObject.Find("TerrainMesh").GetComponent<TerrainCollider>().bounds;
        }
        else
        {
            terrainBounds = GameObject.Find("Terrain").GetComponent<TerrainCollider>().bounds;
        }

        // navMeshData = GameObject.Find("Terrain").GetComponent<NavMeshSurface>().navMeshData;
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && SceneManager.GetActiveScene().name != "Level3")
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
