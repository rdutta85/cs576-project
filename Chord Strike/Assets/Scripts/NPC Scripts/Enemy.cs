using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class Enemy : ChordKnowledge
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

    public Note note;
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
    private TextMeshProUGUI noteText;
    public bool isDead;
    protected NavMeshAgent agent;
    protected NavMeshSurface surface;


    // TODO: get map data from level

    void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();



        // add NAVMESH AGENT and NAVMESH OBSTACLE
        // gameObject.AddComponent<NavMeshAgent>();
        // gameObject.AddComponent<NavMeshObstacle>();

        // gameObject.AddComponent<BoxCollider>();

        // if (gameObject.GetComponent<Rigidbody>() == null)
        //     gameObject.AddComponent<Rigidbody>();

        // if (gameObject.GetComponent<MeshCollider>() == null)
        //     gameObject.AddComponent<MeshCollider>();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        gameObject.AddComponent<NavMeshAgent>();
        gameObject.AddComponent<MeshCollider>();
        gameObject.AddComponent<Rigidbody>();

        TextMeshProUGUI[] textUIs = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI textUI in textUIs)
        {
            if (textUI.name == "ChordText")
            {
                noteText = textUI;
                Debug.Log("ChordText found");
                break;
            }

            Debug.Log("ChordText not found");
        }

        gameObject.layer = 7; // Enemy layer

        //assign a random chord to the enemy
        note = GenerateNote();

        isDead = false;
        currVelocity = 0.0f;
        last_attack = 0f;
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();

        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.agentTypeID = 1;
        // surface = GameObject.Find("NavMeshLevel1").GetComponent<NavMeshSurface>();
        // surface.BuildNavMesh();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isDead) return;

        if (!character_controller.isGrounded)
        {
            float verticalVelocity = -9.81f * Time.deltaTime;
            Vector3 moveDirection = new Vector3(0, verticalVelocity, 0);
            character_controller.Move(moveDirection * Time.deltaTime);
            // return;
        }


        int move = Move();
        AnimateMove(move);
        Attack();
    }

    protected virtual int Move()
    {
        //npc moves towards player globally

        int move;
        RaycastHit hit;
        Vector3 move_direction;
        float move_velocity;
        float dist = Vector3.Distance(transform.position, junko.transform.position);
        if (dist > attackRange / 1.2f)
        {//stay meters away from player
            if (Physics.Raycast(transform.position, junko.transform.position - transform.position, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == junko.gameObject)//sprint if player is in l.o.s
                {
                    // Debug.Log("Player in line of sight");
                    move_velocity = tgtMoveVelocity;
                    move = 2;
                    // agent.speed = tgtMoveVelocity;
                    Vector3 future_junko_pos = junko.transform.position;
                    float delta_pos = Mathf.Infinity;
                    while (delta_pos > 0.01f)
                    {
                        float distance = Vector3.Distance(future_junko_pos, transform.position);
                        float look_ahead_time = distance / move_velocity;
                        Vector3 last_future_junko_pos = future_junko_pos;
                        future_junko_pos = junko.transform.position + junko.GetVelocity() * junko.GetMoveDirection() * look_ahead_time;
                        delta_pos = Vector3.Distance(last_future_junko_pos, future_junko_pos);
                    }
                    move_direction = future_junko_pos - transform.position;
                    move_direction.Normalize();
                }
                else
                {
                    // Debug.Log("Player is NOT in line of sight");
                    move_velocity = rndMoveVelocity;
                    move = 1;
                    // agent.speed = rndMoveVelocity;

                    // move direction is 90, 180 or 270 degrees from the player
                    int angle = Random.Range(0, 3) * 90;
                    move_direction = Quaternion.Euler(0, angle, 0) * (junko.transform.position - transform.position);
                }
            }
            else
            {
                // Debug.Log("Player is NOT in line of sight");
                move_velocity = rndMoveVelocity;
                move = 1;
                // agent.speed = rndMoveVelocity;
                // move direction is 90, 180 or 270 degrees from the player
                int angle = Random.Range(0, 3) * 90;
                move_direction = Quaternion.Euler(0, angle, 0) * (junko.transform.position - transform.position);
            }
        }
        else
        {
            move_velocity = 0f;
            // agent.speed = 0f;
            move = 0;
            move_direction = junko.transform.position - transform.position;

        }

        //rotate model towards move direction
        Quaternion targetRotation = Quaternion.LookRotation(move_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        // agent.SetDestination(junko.transform.position);

        character_controller.Move(move_direction * move_velocity * Time.deltaTime);

        return move;
    }

    protected virtual void AnimateMove(int move)
    {
        if (move == 0)
        {
            animation_controller.SetBool("isWalking", false);
            animation_controller.SetBool("isRunning", false);
        }
        else if (move == 1)
        {
            animation_controller.SetBool("isWalking", true);
            animation_controller.SetBool("isRunning", false);
        }
        else if (move == 2)
        {
            animation_controller.SetBool("isWalking", false);
            animation_controller.SetBool("isRunning", true);
        }
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
    // TODO: fix this function
    {
        //use ChordsToNotes to determine if the player chord matches
        //the enemy note
        string firstLetter = player_chord[0].ToString().ToUpper();
        string rest = player_chord.Substring(1);
        player_chord = firstLetter + rest;

        List<Note> player_notes = chordToNotes[(Chord)System.Enum.Parse(typeof(Chord), player_chord)];

        if (player_notes.Contains(note))
        {
            Debug.Log("Succesful enemy attack with " + player_chord + " on " + note);
            if (health > 0) TakeDamage(dmg);

            if (SceneManager.GetActiveScene().name != "Level1")
                note = GenerateNote();  //change the note of the enemy after being attacked
        }
        else
        {
            Debug.Log("Bad enemy attack with " + player_chord + " on " + note);
            MistakeCounter++;
            if (MistakeCounter == 3)
            {//if the player plays the wrong chord 3 times, the enemy is enraged
                StartCoroutine("Enraged");
                MistakeCounter = 0;
            }
        }
    }

    protected Note GenerateNote()
    {
        //randomly generate chord
        System.Array values = System.Enum.GetValues(typeof(Note));
        Note randomNote = (Note)values.GetValue(Random.Range(0, values.Length));

        string noteStr = randomNote.ToString();
        noteText.text = noteStr[0].ToString().ToUpper() + noteStr.Substring(1).ToLower();

        return randomNote;

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
