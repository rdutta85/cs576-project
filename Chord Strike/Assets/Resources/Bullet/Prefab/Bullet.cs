using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour{
    public Vector3 direction;
    public float velocity;
    public float birth_time;
    
    void Start(){
        velocity = 5f;
    }

    void Update(){
        if(Time.time - birth_time > 5f){
            Destroy(transform.gameObject);
        }
        transform.position += velocity * direction * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Enemy")){
            Destroy(gameObject);
        }else{
            // Destroy(gameObject);
        }
    }
}
