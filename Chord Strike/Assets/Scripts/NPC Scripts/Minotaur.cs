using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();
        animation_controller = GetComponent<Animator>();
        character_controller = GetComponent<CharacterController>();
        attackRange = 3f;
        attackSpeed = 4f;
        tgtMoveVelocity = 1f;
        rndMoveVelocity = 0.5f;
        currVelocity = 0.0f;
        maxHealth = 200f;
        health = maxHealth;
        AttackDamage = new float[]{20f,28f};
        chord = GenerateChord();    
    }

    // Update is called once per frame
    void Update()
    {
        float verticalVelocity = 0f;
        if (character_controller.isGrounded)
            verticalVelocity = -0.1f; 
        else
            verticalVelocity -= 1000f * Time.deltaTime; 
        Vector3 moveDirection = new Vector3(0, verticalVelocity, 0);
        character_controller.Move(moveDirection * Time.deltaTime);
        Move();
        Attack();  
    }

    protected override void Attack(){
        if (Vector3.Distance(transform.position, junko.transform.position) < attackRange && health > 0 && Time.time - last_attack >= attackSpeed){
            animation_controller.SetBool("isWalking", false);
            animation_controller.SetBool("isRunning", false);
            int style = Random.Range(1,5);
            Debug.Log("Attack "+style);
            animation_controller.SetTrigger("Attack " + style);
            junko.TakeDamage(Random.Range(AttackDamage[0],AttackDamage[1]));
            last_attack = Time.time;
        }
    }
}
