using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raptor : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();
        animation_controller = GetComponent<Animator>();
        character_controller = GetComponent<CharacterController>();
        attackRange = 1.1f;
        attackSpeed = 1f;
        tgtMoveVelocity = 1.0f;
        rndMoveVelocity = 0.5f;
        currVelocity = 0.0f;
        maxHealth = 100f;
        health = maxHealth;
        AttackDamage = new float[] { 10f, 15f };
        chord = GenerateChord();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
