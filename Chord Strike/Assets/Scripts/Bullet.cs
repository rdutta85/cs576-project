using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public Vector3 direction;
    public float velocity;
    public float birth_time;
    
    void Start(){

    }

    void Update(){

        transform.position += velocity * direction * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other){
        
        if(other.gameObject.CompareTag("Enemy")){

        }else{
            Destroy(gameObject);
        }
    }
}
