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

    // TODO: get map data from level

    void Awake(){
        healthBar = GetComponentInChildren<HealthBar>();
    }

    // Start is called before the first frame update
    void Start()
    {   
        isDead = false;
        currVelocity = 0.0f;
        last_attack = 0f;
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();
    }

    // Update is called once per frame
    protected void Update()
    {   
        if(isDead) return;
        float verticalVelocity = 0f;
        if (character_controller.isGrounded)
            verticalVelocity = -0.1f; 
        else
            verticalVelocity -= 9.81f * Time.deltaTime; 
        Vector3 moveDirection = new Vector3(0, verticalVelocity, 0);
        character_controller.Move(moveDirection * Time.deltaTime);

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
        if(dist > attackRange/2f){//stay meters away from player
            if (Physics.Raycast(transform.position, junko.transform.position - transform.position, out hit, Mathf.Infinity)){
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
            else{
            // Debug.Log("Player is NOT in line of sight");
            animation_controller.SetBool("isWalking", true);
            animation_controller.SetBool("isRunning", false);
            move_velocity = rndMoveVelocity;
            }
            //rotate model towards move direction
        }else{
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
    protected virtual void Attack(){
        // if player is within range, attack player
        if (Vector3.Distance(transform.position, junko.transform.position) < attackRange && health > 0 && Time.time - last_attack >= attackSpeed){
            animation_controller.SetBool("isWalking", false);
            animation_controller.SetBool("isRunning", false);
            animation_controller.SetTrigger("Attack");
            junko.TakeDamage(Random.Range(AttackDamage[0],AttackDamage[1]));
            last_attack = Time.time;
        }
    }
    public float[] GetAttackDmg(){
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
        // Vector3 direction = target_pos - transform.position;
        // transform.rotation = Quaternion.LookRotation(direction);

        // move towards target smoothly from current to velocity
        currVelocity = Mathf.Lerp(currVelocity, target_velocity, 0.1f);
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

    private void TakeDamage(float dmg){
        health -= dmg;
        healthBar.UpdateHealthBar(health,maxHealth);
        if(health <= 0f){
            StartCoroutine("Death");  
        }
    }

    public void Attacked(string player_chord, float dmg){
        if (true){//use ChordsToNotes to determine if the player chord matches
            if(health > 0) TakeDamage(dmg);
            chord = GenerateChord();//change the note of the enemy after being attacked
        }else{
            MistakeCounter++;
            if(MistakeCounter == 3){//if the player plays the wrong chord 3 times, the enemy is enraged
                StartCoroutine("Enraged");
                MistakeCounter = 0;
            }
        }
    }

    protected string GenerateChord(){//TODO randomly generate chord
        return "";
    }

    //enrages enemies deal 2x damage for 30s
    private IEnumerator Enraged(){
        AttackDamage = new float[] {AttackDamage[0]*2f, AttackDamage[1]*2f};
        yield return new WaitForSeconds(30f);
        AttackDamage = new float[] {AttackDamage[0]/2f, AttackDamage[1]/2f};
    } 
    //play death animation
    protected virtual IEnumerator Death(){
        isDead = true;
        animation_controller.SetTrigger("Death");
        yield return new WaitForSeconds(animation_controller.GetCurrentAnimatorStateInfo(0).length + 1f);
        Destroy(gameObject);
    }
}
