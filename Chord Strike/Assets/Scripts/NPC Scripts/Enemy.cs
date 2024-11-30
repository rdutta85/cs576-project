using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All equivalents have been squashed
enum Chord {
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


    // Start is called before the first frame update
    void Start()
    {
        // Delete this.
        note = "A";

    }

    // Update is called once per frame
    void Update()
    {
        
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
