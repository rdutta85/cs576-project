using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour{
    Vector3 direction;
    float velocity = 10f;
    float lifetime = 0.5f;
    JunkochanControl junko;
    public GameObject owner;
    public Animator anim;
    
    void Start(){
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();
        Destroy(gameObject, lifetime);
    }

    void Update(){
        Vector3 move_direction = junko.transform.position - owner.transform.position;
        transform.position += move_direction * velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other){
        GameObject hit = other.gameObject;
        if(hit == GameObject.Find("JunkoChan")){
            float[] AttackDamage = owner.GetComponent<Enemy>().GetAttackDmg();
            junko.TakeDamage(Random.Range(AttackDamage[0],AttackDamage[1]));
            Destroy(gameObject);
            anim.SetBool("isShooting",false);
        }else{
            // Destroy(gameObject);
        }
    }
}
