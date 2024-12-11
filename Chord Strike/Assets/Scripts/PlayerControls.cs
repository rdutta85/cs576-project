using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour{
    private CharacterController character_controller;
    private Vector3 movement_direction;
    private float velocity;
    private GameObject projectile_template;
    private float projectile_velocity;
    private Vector3 projectile_starting_pos;
    private float lastShootTime = 0f;
    void Start()
    {
        character_controller = GetComponent<CharacterController>();
        movement_direction = new Vector3(0.0f, 0.0f, 0.0f);
        velocity = 5f;

        projectile_template = (GameObject)Resources.Load("Bullet/Prefab/Bullet", typeof(GameObject));
        if(projectile_template == null){
            Debug.LogError("Error: could not find bullet prefab!");

        projectile_velocity = 5.0f;
        projectile_starting_pos = new Vector3(0.0f, 0.0f, 0.0f);

        }
    }

    void Update()
    {

        // movement();
        // shoot();
    }

    private void movement(){
        float ang = Mathf.Deg2Rad * transform.rotation.eulerAngles.y;

        // Update Forward/Back Movement Direction
        float xdirection = Mathf.Sin(ang);
        float zdirection = Mathf.Cos(ang);
        movement_direction = new Vector3(xdirection, 0.0f, zdirection);

        // Update Left Movement Direction
        float leftX = Mathf.Sin(ang + Mathf.PI / 2);
        float leftZ = Mathf.Cos(ang + Mathf.PI / 2);
        Vector3 left = new Vector3(leftX, 0f, leftZ);

        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftControl))
        {
            float turn = Input.GetAxis("Horizontal");
            transform.Rotate(0, turn * 300f * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftControl))
        {
            float turn = Input.GetAxis("Horizontal");
            transform.Rotate(0, turn * 300f * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            character_controller.Move(movement_direction * velocity / 1.5f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            character_controller.Move(movement_direction * -velocity / 1.5f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            character_controller.Move(left * -velocity / 1.5f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            character_controller.Move(left * velocity / 1.5f * Time.deltaTime);
        }


        if (!character_controller.isGrounded)
        {
            character_controller.Move(new Vector3 (0f, -1f, 0f) * 9.81f * Time.deltaTime);
        }
    }

    private void shoot(){
        // if (Input.GetKey(KeyCode.A))
        // {
        //     // Shoot some projectile

        // }
        // else if (Input.GetKey(KeyCode.S))
        // {
        //     // Shoot some projectile

        // }
        // else if (Input.GetKey(KeyCode.D))
        // {
        //     // Shoot some projectile

        // }

        //this is for testing purposes
        // if(Input.GetKey("space") && Time.time - lastShootTime >= 0.5f){

        //     lastShootTime = Time.time;
        //     projectile_starting_pos = transform.position;
        //     GameObject new_object = Instantiate(projectile_template, projectile_starting_pos, Quaternion.identity);
        //     new_object.GetComponent<Bullet>().direction = movement_direction; //should be direction character is facing
        //     new_object.GetComponent<Bullet>().velocity = projectile_velocity;
        //     new_object.GetComponent<Bullet>().birth_time = Time.time;
        // }
    }
}
