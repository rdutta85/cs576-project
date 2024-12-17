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
}
