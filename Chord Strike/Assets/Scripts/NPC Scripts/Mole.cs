using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : Enemy
{
    // Start is called before the first frame update
    private Animator[] animators;
    void Start()
    {
        base.Start();

        animators = GetComponentsInChildren<Animator>();
        character_controller = GetComponent<CharacterController>();
        attackRange = 6.5f;
        attackSpeed = 1.5f;
        tgtMoveVelocity = 1f;
        rndMoveVelocity = 0.5f;
        currVelocity = 0.0f;
        maxHealth = 100f;
        health = maxHealth;
        AttackDamage = new float[] { 15f, 24f };
    }

    // Update is called once per frame
    // void Update()
    // {
    //     float verticalVelocity = 0f;
    //     if (character_controller.isGrounded)
    //         verticalVelocity = -0.1f;
    //     else
    //         verticalVelocity -= 9.81f * Time.deltaTime;
    //     Vector3 moveDirection = new Vector3(0, verticalVelocity, 0);
    //     character_controller.Move(moveDirection * Time.deltaTime);
    //     Move();
    //     Attack();
    // }
    //have to do a custom Move() because the model has two animators
    protected override void AnimateMove(int move)
    {

        if (move == 0)
        {
            animators[0].SetBool("isWalking", false);
            animators[0].SetBool("isRunning", false);
            animators[1].SetBool("isWalking", false);
            animators[1].SetBool("isRunning", false);
        }
        else if (move == 1)
        {
            animators[0].SetBool("isWalking", true);
            animators[0].SetBool("isRunning", false);
            animators[1].SetBool("isWalking", true);
            animators[1].SetBool("isRunning", false);
        }
        else if (move == 2)
        {
            animators[0].SetBool("isWalking", false);
            animators[0].SetBool("isRunning", true);
            animators[1].SetBool("isWalking", false);
            animators[1].SetBool("isRunning", true);
        }
    }

    protected override void Attack()
    {
        // if player is within range, attack player
        if (Vector3.Distance(transform.position, junko.transform.position) < attackRange && health > 0 && Time.time - last_attack >= attackSpeed)
        {
            animators[0].SetBool("isWalking", false);
            animators[0].SetBool("isRunning", false);
            animators[0].SetTrigger("Attack");
            animators[1].SetBool("isWalking", false);
            animators[1].SetBool("isRunning", false);
            animators[1].SetTrigger("Attack");
            junko.TakeDamage(Random.Range(AttackDamage[0], AttackDamage[1]));
            last_attack = Time.time;
        }

    }

    protected override IEnumerator Death()
    {
        animators[0].SetTrigger("Death");
        animators[1].SetTrigger("Death");
        yield return new WaitForSeconds(animators[1].GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
