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

    public string chord;
    protected JunkochanControl junko;
    protected float attackRange;//range of attack
    protected float attackSpeed;//attack cooldown
    protected float last_attack;//time of last attack
    protected float rndMoveVelocity;//out of sight speed
    protected float tgtMoveVelocity;//line of sight speed
    protected float currVelocity;
    protected float maxHealth;
    protected float health;
    protected float[] AttackDamage;//Min and Max damage 
    protected Animator animation_controller;
    protected CharacterController character_controller;
    protected int MistakeCounter;//tracks the number of times the player plays the wrong chord
    private HealthBar healthBar;
    protected bool isDead;

    // Dictionary mapping chords to notes
    private Dictionary<Chord, List<Note>> chordToNotes = new Dictionary<Chord, List<Note>>
    {
        { Chord.A, new List<Note> { Note.A, Note.Cs, Note.E } },
        { Chord.Am, new List<Note> { Note.A, Note.C, Note.E } },
        { Chord.B, new List<Note> { Note.B, Note.Ds, Note.Fs } },
        { Chord.Bm, new List<Note> { Note.B, Note.D, Note.Fs } },
        { Chord.C, new List<Note> { Note.C, Note.E, Note.G } },
        { Chord.Cm, new List<Note> { Note.C, Note.Ef, Note.G } },
        { Chord.D, new List<Note> { Note.D, Note.Fs, Note.A } },
        { Chord.Dm, new List<Note> { Note.D, Note.F, Note.A } },
        { Chord.E, new List<Note> { Note.E, Note.Gs, Note.B } },
        { Chord.Em, new List<Note> { Note.E, Note.G, Note.B } },
        { Chord.F, new List<Note> { Note.F, Note.A, Note.C } },
        { Chord.Fm, new List<Note> { Note.F, Note.Af, Note.C } },
        { Chord.G, new List<Note> { Note.G, Note.B, Note.D } },
        { Chord.Gm, new List<Note> { Note.G, Note.Bf, Note.D } },
        { Chord.Cs, new List<Note> { Note.Cs, Note.E, Note.Gs } },
        { Chord.Csm, new List<Note> { Note.Cs, Note.E, Note.G } },
        { Chord.Af, new List<Note> { Note.Af, Note.C, Note.Ef } },
        { Chord.Afm, new List<Note> { Note.Af, Note.B, Note.Ef } },
        { Chord.Bf, new List<Note> { Note.Bf, Note.D, Note.F } },
        { Chord.Bfm, new List<Note> { Note.Bf, Note.D, Note.Fs } },
        { Chord.Ef, new List<Note> { Note.Ef, Note.G, Note.Bf } },
        { Chord.Efm, new List<Note> { Note.Ef, Note.G, Note.B } },
        { Chord.Fs, new List<Note> { Note.Fs, Note.A, Note.Cs } },
        { Chord.Fsm, new List<Note> { Note.Fs, Note.A, Note.C } },
    };

    // TODO: get map data from level

    void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //assign a random chord to the enemy
        chord = GenerateChord();

        isDead = false;
        currVelocity = 0.0f;
        last_attack = 0f;
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (isDead) return;
        // float verticalVelocity = 0f;
        // if (character_controller.isGrounded)
        //     verticalVelocity = -0.1f;
        // else
        //     verticalVelocity -= 9.81f * Time.deltaTime;
        // Vector3 moveDirection = new Vector3(0, verticalVelocity, 0);
        // character_controller.Move(moveDirection * Time.deltaTime);

        Move();
        Attack();
    }

    protected virtual void Move()
    {
        //npc moves towards player globally
        RaycastHit hit;
        Vector3 move_direction;
        float move_velocity;
        float dist = Vector3.Distance(transform.position, junko.transform.position);
        if (dist > attackRange / 2f)
        {//stay meters away from player
            if (Physics.Raycast(transform.position, junko.transform.position - transform.position, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == junko.gameObject)//sprint if player is in l.o.s
                {
                    Debug.Log("Player in line of sight");
                    animation_controller.SetBool("isWalking", false);
                    animation_controller.SetBool("isRunning", true);
                    move_velocity = tgtMoveVelocity;
                }
                else
                {
                    // Debug.Log("Player is NOT in line of sight");
                    animation_controller.SetBool("isWalking", true);
                    animation_controller.SetBool("isRunning", false);
                    move_velocity = rndMoveVelocity;
                }
            }
            else
            {
                // Debug.Log("Player is NOT in line of sight");
                animation_controller.SetBool("isWalking", true);
                animation_controller.SetBool("isRunning", false);
                move_velocity = rndMoveVelocity;
            }
            //rotate model towards move direction
        }
        else
        {
            move_velocity = 0f;
            animation_controller.SetBool("isWalking", false);
            animation_controller.SetBool("isRunning", false);
        }
        move_direction = junko.transform.position - transform.position;
        move_direction.Normalize();

        //rotate model towards move direction
        Quaternion targetRotation = Quaternion.LookRotation(move_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        character_controller.Move(move_direction * move_velocity * Time.deltaTime);
    }
    protected virtual void Attack()
    {
        // if player is within range, attack player
        if (Vector3.Distance(transform.position, junko.transform.position) < attackRange && health > 0 && Time.time - last_attack >= attackSpeed)
        {
            animation_controller.SetBool("isWalking", false);
            animation_controller.SetBool("isRunning", false);
            animation_controller.SetTrigger("Attack");
            junko.TakeDamage(Random.Range(AttackDamage[0], AttackDamage[1]));
            last_attack = Time.time;
        }
    }
    public float[] GetAttackDmg()
    {
        return AttackDamage;
    }
    float GetMapHeightAtPos(float x, float z)
    {
        return 0.0f; // TODO: update
    }

    //obsolete. now uses character controller to move
    void MoveSmoothly(Vector3 target_pos, float target_velocity)
    {
        // rotate towards target
        Vector3 direction = target_pos - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);

        // move towards target smoothly from current to velocity
        currVelocity = Mathf.Lerp(currVelocity, target_velocity, 0.01f);
        transform.position = Vector3.MoveTowards(transform.position, target_pos, currVelocity * Time.deltaTime);
    }

    /*
    This method should correctly map each chord to a list of notes using the algorithm in Guitar.cs

    This method should be used to check received signals and determine whether or not an attack should damage the enemy
     */
    private List<Note> ChordToNotes()//TODO match player chords to enemy chord
    {
        // Delete this.
        return null;
    }

    private void TakeDamage(float dmg)
    {
        health -= dmg;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0f)
        {
            StartCoroutine("Death");
        }
    }

    public void Attacked(string player_chord, float dmg)
    {
        //use ChordsToNotes to determine if the player chord matches
        //the enemy chord
        if (chordToNotes[(Chord)System.Enum.Parse(typeof(Chord), chord)].Contains((Note)System.Enum.Parse(typeof(Note), player_chord)))
        {
            if (health > 0) TakeDamage(dmg);
            chord = GenerateChord();  //change the note of the enemy after being attacked
        }
        else
        {
            MistakeCounter++;
            if (MistakeCounter == 3)
            {//if the player plays the wrong chord 3 times, the enemy is enraged
                StartCoroutine("Enraged");
                MistakeCounter = 0;
            }
        }
    }

    protected string GenerateChord()
    {
        //randomly generate chord
        System.Array values = System.Enum.GetValues(typeof(Chord));
        Chord randomChord = (Chord)values.GetValue(Random.Range(0, values.Length));
        return randomChord.ToString();

    }

    //enrages enemies deal 2x damage for 30s
    private IEnumerator Enraged()
    {
        AttackDamage = new float[] { AttackDamage[0] * 2f, AttackDamage[1] * 2f };
        yield return new WaitForSeconds(30f);
        AttackDamage = new float[] { AttackDamage[0] / 2f, AttackDamage[1] / 2f };
    }
    //play death animation
    protected virtual IEnumerator Death()
    {
        isDead = true;
        animation_controller.SetTrigger("Death");
        yield return new WaitForSeconds(animation_controller.GetCurrentAnimatorStateInfo(0).length + 1f);
        Destroy(gameObject);
    }
}
