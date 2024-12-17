using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Enemy
{
    // Start is called before the first frame update
    private float projectileRange = 4.7f;
    private GameObject projectile_template;
    void Start()
    {
        base.Start();

        projectile_template = (GameObject)Resources.Load("Bullet/Prefab/Sphere", typeof(GameObject));
        Animator[] animators = GetComponentsInChildren<Animator>();
        animation_controller = animators[0];
        character_controller = GetComponent<CharacterController>();
        attackRange = 5.0f;
        attackSpeed = 4.6f;
        tgtMoveVelocity = 1.5f;
        rndMoveVelocity = 0.75f;
        currVelocity = 0.0f;
        maxHealth = 75f;
        health = maxHealth;
        AttackDamage = new float[] { 12f, 17f };
    }

    protected override void AnimateMove(int move)
    {
        // if(dist > 4.5f){//stay at least 4.5 meters away from player
        // float dist = Vector3.Distance(transform.position, junko.transform.position);
        base.AnimateMove(move);
    }
    protected override void Attack()
    {
        float dist = Vector3.Distance(transform.position, junko.transform.position);
        if (dist <= projectileRange && dist >= attackRange && health > 0)
        {
            Shoot();
        }
        else if (dist < attackRange && health > 0 && Time.time - last_attack >= attackSpeed)
        {
            if (Vector3.Distance(transform.position, junko.transform.position) < attackRange && health > 0)
            {
                animation_controller.SetBool("isWalking", false);
                animation_controller.SetBool("isRunning", false);
                animation_controller.SetTrigger("Attack");
                junko.TakeDamage(Random.Range(AttackDamage[0], AttackDamage[1]));
                last_attack = Time.time;
            }
        }
    }
    private float last_shot;
    private void Shoot()
    {
        if (Time.time - last_shot >= attackSpeed)
        {
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
