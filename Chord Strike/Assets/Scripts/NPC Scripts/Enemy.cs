using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All equivalents have been squashed
enum Chord
{
    A, B, C, D, E, F, G, Am, Bm, Cm, Dm, Em, Fm, Gm,
    Af, Bf, Ef, Afm, Bfm, Efm,
    Cs, Fs, Csm, Fsm
};

// These are all possible note names. 
enum Note
{
    A, B, C, D, E, F, G,
    Af, Bf, Cf, Df, Ef, Ff, Gf,
    As, Bs, Cs, Ds, Es, Fs, Gs
}

public class Enemy : MonoBehaviour
{
    /*
     Tasks for Tele:
     Write ChordToNoteS() using the algorithm described in Guitar.cs

     When Junko plays a chord, there will be a listener that will accept the chord. Your goal is to essentially implement the health and damage system
     for both the enemies (optional, you can just destroy the object once hit) and the player (health is also optional, but it would make the game hard). 
     */

    /*
     Tasks for R_Dutta 
     Create basic enemy spawning and pathfinding in Update().
     */

    public string note;
    private JunkochanControl junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();
    private attackRange = 5.0f;
    private float rndMoveVelocity = 0.5f;
    private float tgtMovevelocity = 1.0f;
    private float currVelocity;

    // TODO: get map data from level

    // Start is called before the first frame update
    void Start()
    {
        currVelocity = 0.0f;
        // Delete this.
        note = "A";

    }

    // Update is called once per frame
    void Update()
    {
        MoveAndAttack();
    }

    void MoveAndAttack()
    {
        // check if player is in line of sight
        RaycastHit hit;
        if (Physics.Raycast(transform.position, junko.transform.position - transform.position, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == junko.gameObject)
            {
                // player is in line of sight move towards player
                MoveSmoothly(junko.transform.position, tgtMovevelocity);

                // if player is within range, attack player
                if (Vector3.Distance(transform.position, junko.transform.position) < attackRange)
                {
                    Damage();
                }
            }
            else
            {
                // player is not in line of sight move randomly
                float rndX = Random.Range(-20.0f, 20.0f);
                float rndZ = Random.Range(-20.0f, 20.0f);
                float rndY = GetMapHeightAtPos(rndX, rndZ);

                MoveSmoothly(new Vector3(rndX, rndY, rndZ), rndMoveVelocity);
            }
        }
        else
        {
            // player is not in line of sight move randomly
            float rndX = Random.Range(-20.0f, 20.0f);
            float rndZ = Random.Range(-20.0f, 20.0f);
            float rndY = GetMapHeightAtPos(rndX, rndZ);

            MoveSmoothly(new Vector3(rndX, rndY, rndZ), rndMoveVelocity);
        }
    }

    void GetMapHeightAtPos(float x, float z)
    {
        return 0.0f; // TODO: update
    }

    void MoveSmoothly(Vector3 target_pos, float target_velocity)
    {
        // rotate towards target
        Vector3 direction = target_pos - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);

        // move towards target smoothly from current to velocity
        currVelocity = Mathf.Lerp(currVelocity, target_velocity, 0.1f);
        transform.position = Vector3.MoveTowards(transform.position, target_pos, currVelocity * Time.deltaTime);
    }

    /*
    This method should correctly map each chord to a list of notes using the algorithm in Guitar.cs

    This method should be used to check received signals and determine whether or not an attack should damage the enemy
     */
    private List<Note> ChordToNotes()
    {
        // Delete this.
        return null;
    }

    public void Damage()
    {
        junko.Damage();
    }
}
