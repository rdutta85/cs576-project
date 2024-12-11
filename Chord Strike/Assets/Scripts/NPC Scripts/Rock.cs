using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{   
    private Rigidbody rb;
    private Collider collider;
    public GameObject hand;
    private JunkochanControl junko;

    private float birth_time;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();
        transform.parent = hand.transform;
        rb.useGravity = false;
        GetComponent<Collider>().isTrigger = true;
        birth_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - birth_time > 8f){
            Destroy(gameObject);
        }
    }


    //throws rock
    public void Release(){
        transform.parent = null;

        rb.useGravity = true;
        transform.rotation = hand.transform.rotation;
        GetComponent<Collider>().isTrigger = false;

        Vector3 last_future_pos;
        Vector3 future_pos = junko.transform.position;
        float delta_pos = float.MaxValue;
        float dist = Vector3.Distance(transform.position, junko.transform.position);

        while(delta_pos > 0.003){
            dist = Vector3.Distance(transform.position, junko.transform.position);
            float look_ahead_time = dist/(100f);
            last_future_pos = future_pos;
            future_pos = junko.transform.position + (look_ahead_time * (junko.GetVelocity() * junko.GetMoveDirection()));
            delta_pos = Vector3.Distance(future_pos, last_future_pos);
        }

        Vector3 move_direction = (future_pos - transform.position);
        move_direction.Normalize();

        rb.AddForce(move_direction * (dist)*100f);
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject == GameObject.Find("JunkoChan"))
        {
            StartCoroutine("Hit");
            junko.TakeDamage(55);
        }
    }

    private IEnumerator Hit(){
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
