using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGiant : Enemy
{
    // Start is called before the first frame update
    public GameObject rock_prefab;
    private GameObject throwing_rock;
    private bool isAttacking;
    void Start()
    {
        base.Start();

        animation_controller = GetComponent<Animator>();
        character_controller = GetComponent<CharacterController>();
        attackRange = 6f;
        attackSpeed = 10f;
        tgtMoveVelocity = 2.5f;
        rndMoveVelocity = 1.5f;
        currVelocity = 0.0f;
        maxHealth = 1000f;
        health = maxHealth;
        isAttacking = false;
        AttackDamage = new float[] { 50f, 55f };
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        float verticalVelocity = 0f;
        if (character_controller.isGrounded)
            verticalVelocity = -0.1f;
        else
            verticalVelocity -= 9.81f * Time.deltaTime;
        Vector3 moveDirection = new Vector3(0, verticalVelocity, 0);
        character_controller.Move(moveDirection * Time.deltaTime);
        if (!animation_controller.GetCurrentAnimatorStateInfo(0).IsName("Spawn") && !isAttacking)
        {
            Move();
            Attack();
        }

    }
    protected virtual void Move()
    {
        //npc moves towards player globally
        RaycastHit hit;
        Vector3 move_direction;
        float move_velocity;
        float dist = Vector3.Distance(transform.position, junko.transform.position);
        if (dist > attackRange / 2f)
        {//stay meters away from player
            move_velocity = tgtMoveVelocity;
            animation_controller.SetBool("isWalking", true);
        }
        else
        {
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

    protected override void Attack()
    {
        if (Vector3.Distance(transform.position, junko.transform.position) < attackRange && health > 0 && Time.time - last_attack >= attackSpeed)
        {
            animation_controller.SetBool("isWalking", false);
            animation_controller.SetBool("isRunning", false);
            int style = Random.Range(1, 3);
            if (style == 1)
            {
                animation_controller.SetTrigger("Stomp");
            }
            else
            {
                animation_controller.SetTrigger("Jump Attack");
            }
            junko.TakeDamage(Random.Range(AttackDamage[0], AttackDamage[1]));
            last_attack = Time.time;
            isAttacking = true;
            StartCoroutine("Attacking");
        }
        else if (Vector3.Distance(transform.position, junko.transform.position) > attackRange + 4 && health > 0 && Time.time - last_attack >= attackSpeed)
        {
            animation_controller.SetBool("isWalking", false);
            animation_controller.SetBool("isRunning", false); // TODO: Parameter 'isRunning' does not exist.
            animation_controller.SetTrigger("Throw");
            last_attack = Time.time;
            isAttacking = true;
            StartCoroutine("Attacking");
        }
    }

    void CreateRock()
    {
        GameObject start_pos = FindChildByName(gameObject, "Empty");
        throwing_rock = Instantiate(rock_prefab, start_pos.transform.position, Quaternion.identity);
        throwing_rock.GetComponent<Rock>().hand = start_pos;
    }

    void ThrowRock()
    {
        throwing_rock.GetComponent<Rock>().Release();
    }
    private GameObject FindChildByName(GameObject parent, string name)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }

            GameObject result = FindChildByName(child.gameObject, name);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    private IEnumerator Attacking()
    {
        while (animation_controller.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Giant@Walk01 - Forward" ||
        animation_controller.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Giant@Idle01")
        {
            yield return null;
        }
        yield return new WaitForSeconds(animation_controller.GetCurrentAnimatorClipInfo(0)[0].clip.length + 2f);
        isAttacking = false;
    }


}
