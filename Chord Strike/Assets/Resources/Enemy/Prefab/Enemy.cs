using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    private float health_points;
    private float regen_speed;
    private float last_damage_time;
    private float velocity;
    // Start is called before the first frame update
    void Start()
    {
        health_points = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - last_damage_time > 10f){
            StartCoroutine(regen());
            last_damage_time = Time.time;
        }
    }

    public void damage(float dmg){
        health_points -= dmg;
        if(health_points <= 0f){
            Destroy(gameObject);
        }
    }
    IEnumerator regen(){
        health_points += 1f;
        yield return new WaitForSeconds(1f);
    }
}
