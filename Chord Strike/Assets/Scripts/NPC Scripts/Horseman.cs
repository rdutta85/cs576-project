using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horseman : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        animation_controller = GetComponent<Animator>();
        character_controller = GetComponent<CharacterController>();
        attackRange = 3.5f;
        attackSpeed = 3f;
        tgtMoveVelocity = 0.75f;
        rndMoveVelocity = 0.5f;
        currVelocity = 0.0f;
        maxHealth = 300f;
        health = maxHealth;
        AttackDamage = new float[] { 24f, 33f };
    }

    // Update is called once per frame
    // void Update(){
    //     float verticalVelocity = 0f;
    //     if (character_controller.isGrounded)
    //         verticalVelocity = -0.1f; 
    //     else
    //         verticalVelocity -= 1000f * Time.deltaTime; 
    //     Vector3 moveDirection = new Vector3(0, verticalVelocity, 0);
    //     character_controller.Move(moveDirection * Time.deltaTime);
    //     Move();
    //     Attack();
    // }
    // protected override void AnimateMove(int move)
    // {
    //     if (move == 0)
    //     {
    //         animation_controller.SetBool("isWalking", false);
    //         animation_controller.SetBool("isRunning", false);
    //     }
    //     else if (move == 1)
    //     {
    //         animation_controller.SetBool("isWalking", true);
    //         animation_controller.SetBool("isRunning", false);
    //     }
    //     else if (move == 2)
    //     {
    //         animation_controller.SetBool("isWalking", false);
    //         animation_controller.SetBool("isRunning", true);
    //     }
    //     //npc moves towards player globally
    //     // Vector3 move_direction;
    //     // float move_velocity;
    //     // float dist = Vector3.Distance(transform.position, junko.transform.position);
    //     // if(dist > 3f){//stay 3m away
    //     //     animation_controller.SetBool("isWalking", true);
    //     //     move_velocity = tgtMoveVelocity;
    //     // }else{
    //     //     move_velocity = 0f;
    //     //     animation_controller.SetBool("isWalking", false);
    //     // }


    // }
}
