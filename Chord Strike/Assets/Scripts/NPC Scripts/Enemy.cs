using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




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

    public Chord chord;
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
    private HealthBar healthBar;    // health bar for the enemy
    private TextMeshProUGUI chordText;
    public bool isDead;
    private NavMeshAgent agent;

    // Dictionary mapping chords to notes
    public Dictionary<Chord, List<Note>> chordToNotes = new Dictionary<Chord, List<Note>>
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
        { Chord.Cs, new List<Note> { Note.Cs, Note.Es, Note.Gs } },
        { Chord.Csm, new List<Note> { Note.Cs, Note.E, Note.Gs } },
        { Chord.Af, new List<Note> { Note.Af, Note.C, Note.Ef } },
        { Chord.Afm, new List<Note> { Note.Af, Note.Cf, Note.Ef } },
        { Chord.Bf, new List<Note> { Note.Bf, Note.D, Note.F } },
        { Chord.Bfm, new List<Note> { Note.Bf, Note.Df, Note.F } },
        { Chord.Ef, new List<Note> { Note.Ef, Note.G, Note.Bf } },
        { Chord.Efm, new List<Note> { Note.Ef, Note.Gf, Note.Bf } },
        { Chord.Fs, new List<Note> { Note.Fs, Note.As, Note.Cs } },
        { Chord.Fsm, new List<Note> { Note.Fs, Note.A, Note.Cs } },
    };

    // All equivalents have been squashed
    public enum Chord
    {
        A, B, C, D, E, F, G, Am, Bm, Cm, Dm, Em, Fm, Gm,
        Af, Bf, Ef, Afm, Bfm, Efm,
        Cs, Fs, Csm, Fsm
    };


    // These are all possible note names. 
    public enum Note
    {
        A, B, C, D, E, F, G,
        Af, Bf, Cf, Df, Ef, Ff, Gf,
        As, Bs, Cs, Ds, Es, Fs, Gs
    }


    // TODO: get map data from level

    void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();

        // add NAVMESH AGENT and NAVMESH OBSTACLE
        gameObject.AddComponent<NavMeshAgent>();
        // gameObject.AddComponent<NavMeshObstacle>();

        // gameObject.AddComponent<BoxCollider>();

        if (gameObject.GetComponent<Rigidbody>() == null)
            gameObject.AddComponent<Rigidbody>();

        if (gameObject.GetComponent<MeshCollider>() == null)
            gameObject.AddComponent<MeshCollider>();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        TextMeshProUGUI[] textUIs = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI textUI in textUIs)
        {
            if (textUI.name == "ChordText")
            {
                chordText = textUI;
                Debug.Log("ChordText found");
                break;
            }

            Debug.Log("ChordText not found");
        }

        gameObject.layer = 7; // Enemy layer

        //assign a random chord to the enemy
        chord = GenerateChord();

        isDead = false;
        currVelocity = 0.0f;
        last_attack = 0f;
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();

        agent = gameObject.GetComponent<NavMeshAgent>();

        string chordStr = chord.ToString();
        chordText.text = chordStr[0].ToString().ToUpper() + chordStr.Substring(1).ToLower();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (isDead) return;

        if (!character_controller.isGrounded)
        {
            float verticalVelocity = 9.81f * Time.deltaTime;
            Vector3 moveDirection = new Vector3(0, verticalVelocity, 0);
            character_controller.Move(moveDirection * Time.deltaTime);
            return;
        }

        Move();
        Attack();
    }

    protected virtual void Move()
    {
        //npc moves towards player globally
        RaycastHit hit;
        Vector3 move_direction;
        // float move_velocity;
        float dist = Vector3.Distance(transform.position, junko.transform.position);
        if (dist > attackRange / 1.2f)
        {//stay meters away from player
            if (Physics.Raycast(transform.position, junko.transform.position - transform.position, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == junko.gameObject)//sprint if player is in l.o.s
                {
                    // Debug.Log("Player in line of sight");
                    animation_controller.SetBool("isWalking", false);
                    animation_controller.SetBool("isRunning", true);
                    // move_velocity = tgtMoveVelocity;
                    agent.speed = tgtMoveVelocity;
                }
                else
                {
                    // Debug.Log("Player is NOT in line of sight");
                    animation_controller.SetBool("isWalking", true);
                    animation_controller.SetBool("isRunning", false);
                    // move_velocity = rndMoveVelocity;
                    agent.speed = rndMoveVelocity;
                }
            }
            else
            {
                // Debug.Log("Player is NOT in line of sight");
                animation_controller.SetBool("isWalking", true);
                animation_controller.SetBool("isRunning", false);
                // move_velocity = rndMoveVelocity;
                agent.speed = rndMoveVelocity;
            }
            //rotate model towards move direction
        }
        else
        {
            // move_velocity = 0f;
            agent.speed = 0f;
            animation_controller.SetBool("isWalking", false);
            animation_controller.SetBool("isRunning", false);
        }
        move_direction = junko.transform.position - transform.position;
        move_direction.Normalize();

        //rotate model towards move direction
        Quaternion targetRotation = Quaternion.LookRotation(move_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        agent.SetDestination(junko.transform.position);

        Debug.Log("path status" + agent.pathStatus);
        Debug.Log("path stale" + agent.isPathStale);

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
        string firstLetter = player_chord[0].ToString().ToUpper();
        string rest = player_chord.Substring(1);
        player_chord = firstLetter + rest;

        if (chordToNotes[chord].Contains((Note)System.Enum.Parse(typeof(Note), player_chord)))
        {
            Debug.Log("Succesful enemy attack with " + player_chord + " on " + chord);
            if (health > 0) TakeDamage(dmg);

            if (SceneManager.GetActiveScene().name != "Level1")
                chord = GenerateChord();  //change the note of the enemy after being attacked
        }
        else
        {
            Debug.Log("Bad enemy attack with " + player_chord + " on " + chord);
            MistakeCounter++;
            if (MistakeCounter == 3)
            {//if the player plays the wrong chord 3 times, the enemy is enraged
                StartCoroutine("Enraged");
                MistakeCounter = 0;
            }
        }
    }

    protected Chord GenerateChord()
    {
        //randomly generate chord
        System.Array values = System.Enum.GetValues(typeof(Chord));
        Chord randomChord = (Chord)values.GetValue(Random.Range(0, values.Length));
        return randomChord; //.ToString().ToUpperInvariant();

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
