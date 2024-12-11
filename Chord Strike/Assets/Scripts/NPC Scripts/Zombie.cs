using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{

    // Start is called before the first frame update
    void Start(){
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();
        animation_controller = GetComponent<Animator>();
        character_controller = GetComponent<CharacterController>();
        attackRange = 1.1f;
        attackSpeed = 1f;
        tgtMoveVelocity = 0.5f;
        rndMoveVelocity = 0.25f;
        currVelocity = 0.0f;
        maxHealth = 100f;
        health = maxHealth;
        AttackDamage = new float[]{10f,20f};
        chord = GenerateChord();
    }

    // Update is called once per frame
    void Update(){
        base.Update();
    }
}
