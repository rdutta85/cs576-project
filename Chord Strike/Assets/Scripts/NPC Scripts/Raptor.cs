using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raptor : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        animation_controller = gameObject.GetComponent<Animator>();
        character_controller = gameObject.GetComponent<CharacterController>();
        attackRange = 5.0f;
        attackSpeed = 4f;
        tgtMoveVelocity = 0.1f;
        rndMoveVelocity = 0.5f;
        currVelocity = 0.0f;
        maxHealth = 100f;
        health = maxHealth;
        AttackDamage = new float[] { 5f, 10f };
    }

}
