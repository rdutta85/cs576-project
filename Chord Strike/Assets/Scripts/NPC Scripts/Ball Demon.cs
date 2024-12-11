using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Enemy
{
    // Start is called before the first frame update
    private float projectileRange = 4.7f;
    private GameObject projectile_template;
    void Start(){
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();
        projectile_template = (GameObject)Resources.Load("Bullet/Prefab/Sphere", typeof(GameObject));
        Animator[] animators = GetComponentsInChildren<Animator>();
        animation_controller = animators[0];
        character_controller = GetComponent<CharacterController>();
        attackRange = 1.5f;
        attackSpeed = 1.6f;
        tgtMoveVelocity = 1.5f;
        rndMoveVelocity = 0.75f;
        currVelocity = 0.0f;
        maxHealth = 75f;
        health = maxHealth;
        AttackDamage = new float[]{12f,17f};
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
        //for initial starter animation
        if(!animation_controller.GetCurrentAnimatorStateInfo(0).IsName("Spawn")){
            Move();
            Attack();
        }
    }
    
    protected override void Move(){
        float dist = Vector3.Distance(transform.position, junko.transform.position);
        Vector3 move_direction;
        float move_velocity = 0f;
        if(dist > 4.5f){//stay at least 4.5 meters away from player
            RaycastHit hit;
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
        Quaternion targetRotation = Quaternion.LookRotation(move_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        character_controller.Move(move_direction * move_velocity * Time.deltaTime);
    }
    protected override void Attack(){
        float dist = Vector3.Distance(transform.position, junko.transform.position);
        if(dist <= projectileRange && dist >= attackRange && health > 0){
            Shoot();
        }else if(dist < attackRange && health > 0 && Time.time - last_attack >= attackSpeed){
            if (Vector3.Distance(transform.position, junko.transform.position) < attackRange && health > 0){
                animation_controller.SetBool("isWalking", false);
                animation_controller.SetBool("isRunning", false);
                animation_controller.SetTrigger("Attack");
                junko.TakeDamage(Random.Range(AttackDamage[0],AttackDamage[1]));
                last_attack = Time.time;
            }   
        }
    }
    private float last_shot;
    private void Shoot(){
        if(Time.time - last_shot >= attackSpeed){
            animation_controller.SetBool("isWalking", false);
            animation_controller.SetBool("isRunning", false);
            animation_controller.SetTrigger("Shoot");
            Vector3 starting_pos = transform.position;
            starting_pos.y = 1f;
            GameObject bullet = Instantiate(projectile_template, starting_pos, Quaternion.identity);
            bullet.GetComponent<Bullet>().owner = gameObject;
            bullet.GetComponent<Bullet>().anim = animation_controller;
            last_shot = Time.time;
        }
    }

}
