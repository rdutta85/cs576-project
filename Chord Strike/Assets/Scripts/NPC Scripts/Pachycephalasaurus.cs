using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pachycephalasaurus : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        animation_controller = GetComponent<Animator>();
        character_controller = GetComponent<CharacterController>();
        attackRange = 3.5f;
        attackSpeed = 2.5f;
        tgtMoveVelocity = 1.5f;
        rndMoveVelocity = 0.75f;
        currVelocity = 0.0f;
        maxHealth = 150f;
        health = maxHealth;
        AttackDamage = new float[] { 15f, 22f };
    }

}

