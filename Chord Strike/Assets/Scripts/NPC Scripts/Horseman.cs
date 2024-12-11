using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horseman : Enemy
{
    // Start is called before the first frame update
    void Start(){
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();
        animation_controller = GetComponent<Animator>();
        character_controller = GetComponent<CharacterController>();
        attackRange = 3.5f;
        attackSpeed = 3f;
        tgtMoveVelocity = 0.75f;
        rndMoveVelocity = 0.5f;
        currVelocity = 0.0f;
        maxHealth = 300f;
        health = maxHealth;
        AttackDamage = new float[]{24f,33f};
        chord = GenerateChord();
    }

    // Update is called once per frame
    void Update(){
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
    protected override void Move(){
        //npc moves towards player globally
        Vector3 move_direction;
        float move_velocity;
        float dist = Vector3.Distance(transform.position, junko.transform.position);
        if(dist > 3f){//stay 3m away
            animation_controller.SetBool("isWalking", true);
            move_velocity = tgtMoveVelocity;
        }else{
            move_velocity = 0f;
            animation_controller.SetBool("isWalking", false);
        }

        move_direction = junko.transform.position - transform.position;
        move_direction.Normalize();

        //rotate model towards move direction
        Quaternion targetRotation = Quaternion.LookRotation(move_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        character_controller.Move(move_direction * move_velocity * Time.deltaTime);
    }
}
