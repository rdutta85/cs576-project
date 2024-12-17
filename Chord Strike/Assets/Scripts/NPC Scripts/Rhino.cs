using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : Enemy
{
    // Start is called before the first frame update
    private float chargeRange = 10;
    private float chargeCooldown = 8f;
    private float last_charge = -8f;
    private bool isCharging;
    private bool isStunned;
    void Start()
    {
        base.Start();

        animation_controller = GetComponent<Animator>();
        character_controller = GetComponent<CharacterController>();
        attackRange = 4f;
        attackSpeed = 2f;
        tgtMoveVelocity = 2.5f;
        rndMoveVelocity = 1.5f;
        currVelocity = 0.0f;
        maxHealth = 200f;
        health = maxHealth;
        isCharging = false;
        isStunned = false;
        AttackDamage = new float[] { 12f, 18f };
    }

    // Update is called once per frame
    protected override void Update()
    {
        float verticalVelocity = 0f;
        if (character_controller.isGrounded)
            verticalVelocity = -0.1f;
        else
            verticalVelocity -= 1000f * Time.deltaTime;
        Vector3 moveDirection = new Vector3(0, verticalVelocity, 0);
        character_controller.Move(moveDirection * Time.deltaTime);
        if (!isCharging && !isStunned) Move();
        Attack();
    }
    protected virtual void Move()
    {
        float dist = Vector3.Distance(transform.position, junko.transform.position);
        Vector3 move_direction;
        float move_velocity = 0f;
        if (dist > 6f)
        {//stay at least 6 meters away from player
            RaycastHit hit;
            if (Physics.Raycast(transform.position, junko.transform.position - transform.position, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == junko.gameObject)//sprint if player is in l.o.s
                {
                    Debug.Log("Player in line of sight");
                    animation_controller.SetBool("isWalking", false);
                    animation_controller.SetBool("isRunning", true);
                    animation_controller.SetBool("isCharging", false);
                    move_velocity = tgtMoveVelocity;
                }
                else
                {
                    // Debug.Log("Player is NOT in line of sight");
                    animation_controller.SetBool("isWalking", true);
                    animation_controller.SetBool("isRunning", false);
                    animation_controller.SetBool("isCharging", false);
                    move_velocity = rndMoveVelocity;
                }
            }
            else
            {
                // Debug.Log("Player is NOT in line of sight");
                animation_controller.SetBool("isWalking", true);
                animation_controller.SetBool("isRunning", false);
                animation_controller.SetBool("isCharging", false);
                move_velocity = rndMoveVelocity;
            }
            //rotate model towards move direction
            move_direction = junko.transform.position - transform.position;
            move_direction.Normalize();
        }
        else if (dist < 6 && dist > 4)
        {//stay still
            animation_controller.SetBool("isWalking", false);
            animation_controller.SetBool("isRunning", false);
            animation_controller.SetBool("isCharging", false);
            move_direction = junko.transform.position - transform.position;
            move_direction.Normalize();
            move_velocity = 0f;
        }
        else
        {//run away from the player
            move_direction = transform.position - junko.transform.position;
            move_direction.Normalize();
            move_velocity = tgtMoveVelocity;
            animation_controller.SetBool("isWalking", false);
            animation_controller.SetBool("isRunning", true);
            animation_controller.SetBool("isCharging", false);
        }
        Quaternion targetRotation = Quaternion.LookRotation(move_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        character_controller.Move(move_direction * move_velocity * Time.deltaTime);
    }
    //two attacks: headswing and charge
    protected override void Attack()
    {
        float dist = Vector3.Distance(transform.position, junko.transform.position);
        if (health > 0 && dist > attackRange && dist <= chargeRange && Time.time - last_charge >= chargeCooldown)
        {
            StartCoroutine("Charge");
            last_charge = Time.time;
        }
    }

    private IEnumerator Charge()
    {
        animation_controller.SetBool("isCharging", true);
        isCharging = true;
        Vector3 move_direction = (junko.transform.position - transform.position);
        move_direction.Normalize();
        Vector3 chargeTarget = transform.position + move_direction * 5f;//charge for up to 12m
        Vector3 direction_before_charge = move_direction;
        transform.rotation = Quaternion.LookRotation(direction_before_charge);
        float time_elapsed = 0;
        while (Vector3.Distance(transform.position, junko.transform.position) > 0.1f && !isStunned && time_elapsed < 2.5f)
        {
            character_controller.Move(direction_before_charge * 5f * Time.deltaTime);
            time_elapsed += Time.deltaTime;
            yield return null;
        }
        isCharging = false;
        animation_controller.SetBool("isCharging", false);
    }
    private IEnumerator Stunned()
    {
        isStunned = true;
        animation_controller.SetTrigger("Stunned");
        yield return new WaitForSeconds(animation_controller.GetCurrentAnimatorStateInfo(0).length);
        animation_controller.SetBool("isCharging", false);
        isCharging = false;
        isStunned = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObject.Find("JunkoChan") && isCharging)
        {
            Debug.Log("Bam " + Time.time);
            StartCoroutine("Stunned");
        }
    }
}
