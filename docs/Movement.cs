using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController character_controller;
    private Vector3 movement_direction;
    private float velocity;
    public bool projectile;
    void Start()
    {
        character_controller = GetComponent<CharacterController>();
        movement_direction = new Vector3(0.0f, 0.0f, 0.0f);
        velocity = 5f;
    }

    void Update()
    {
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

        if (Input.GetKey(KeyCode.A))
        {
            // Shoot some projectile
            projectile = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            // Shoot some projectile
            projectile = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // Shoot some projectile
            projectile = true;
        }


        if (!character_controller.isGrounded)
        {
            character_controller.Move(new Vector3 (0f, -1f, 0f) * 9.81f * Time.deltaTime);
        }

    }
}
