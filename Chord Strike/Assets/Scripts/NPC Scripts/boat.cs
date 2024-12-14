using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boat : MonoBehaviour
{
    private TerrainCollider terrainCollider;
    private JunkochanControl junkochanControl;
    private Bounds terrainBounds;

    // Start is called before the first frame update
    void Start()
    {
        terrainCollider = GameObject.Find("TerrainMesh").GetComponent<TerrainCollider>();

        terrainBounds = terrainCollider.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (junkochanControl == null)
        {
            junkochanControl = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();
            if (junkochanControl == null) return;
        }

        // if Junkochan is in site, move the boat towards Junkochan
        RaycastHit hit;
        if (Physics.Raycast(transform.position, junkochanControl.transform.position - transform.position, out hit, 1000))
        {
            if (hit.collider.gameObject.name == "JunkoChan")
            {
                Debug.Log("Junkochan is in site!");

                // change the boat's orientation to face Junkochan smoothly
                Vector3 targetDirection = junkochanControl.transform.position - transform.position;
                float step = 0.1f;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);

                // move the boat towards Junkochan
                transform.position = Vector3.MoveTowards(transform.position, junkochanControl.transform.position, 0.1f);
                return;
            }
        }

        Debug.Log("Moving randomly");

        // Otherwise, move the boat randomly within the terrain bounds
        Vector3 randomPosition = new Vector3(
            Random.Range(terrainBounds.min.x, terrainBounds.max.x),
            transform.position.y,
            Random.Range(terrainBounds.min.z, terrainBounds.max.z)
        );
        transform.position = Vector3.MoveTowards(transform.position, randomPosition, 0.1f);

        // change the boat's orientation to face the direction it is moving
        Vector3 targetDir = randomPosition - transform.position;
        float step2 = 0.1f;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step2, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);

    }
}
