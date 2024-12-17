using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        animation_controller = GetComponent<Animator>();
        character_controller = GetComponent<CharacterController>();
        attackRange = 2f;
        attackSpeed = 2f;
        tgtMoveVelocity = 0.5f;
        rndMoveVelocity = 0.25f;
        currVelocity = 0.0f;
        maxHealth = 100f;
        health = maxHealth;
        AttackDamage = new float[] { 10f, 20f };
    }

}
