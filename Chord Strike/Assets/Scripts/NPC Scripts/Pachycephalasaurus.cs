using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pachycephalasaurus : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();
        animation_controller = GetComponent<Animator>();
        character_controller = GetComponent<CharacterController>();
        attackRange = 1.5f;
        attackSpeed = 1.5f;
        tgtMoveVelocity = 1.5f;
        rndMoveVelocity = 0.75f;
        currVelocity = 0.0f;
        maxHealth = 150f;
        health = maxHealth;
        AttackDamage = new float[]{15f,22f};
        chord = GenerateChord();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}

