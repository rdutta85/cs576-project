using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    // Script should be attached to Main Camera
    private int cam;

    private GameObject obj;

    private GameObject player; // Junkochan

    private GameObject boss;

    private Vector3 offset;
    private bool switched = false;
    // Start is called before the first frame update
    void Start()
    {
        cam = 0; // Basic cam

        player = GameObject.Find("JunkoChan");
        boss = GameObject.Find("Golem(Boss)");

        obj = GameObject.Find("Main Camera");

        offset = player.transform.position - obj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandleScrollInput();  // Used so that player can not go off screen
        if (Input.GetKeyDown(KeyCode.C)) // Key C to switch between cams
        {
            HandleSwitch();
            switched = true;
        }
        if (cam == 0)
        {
            // Unlock to player
            obj.transform.position = player.transform.position - offset;
            obj.transform.LookAt(player.transform.position);
        }
        else
        {
            // Lock on boss
            obj.transform.position = player.transform.position - offset;
            obj.transform.LookAt(boss.transform.position);
        }

        switched = false;
    }

    // Function used to switch between the cam to focus on player vs cam to focus on boss
    private void HandleSwitch()
    {
        // If on player cam then switch to boss
        if (cam == 0)
        {
            cam = 1;
        }

        // If on boss cam then switch to player
        else if (cam == 1)
        {
            cam = 0;
        }
    }

    private void HandleScrollInput()
    {
        // Scrolling upwards
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            // Change the range of view to be wider
            offset += new Vector3(1f, 1f, 1f);
        }

        // Scrolling downwards
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            // Change the range of view to be more narrow
            offset -= new Vector3(1f, 1f, 1f);
        }
    }
}
