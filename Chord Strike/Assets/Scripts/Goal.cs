using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Goal3 : MonoBehaviour
{
    public Material red;
    public Material green;
    public GameObject obj;
    private GameObject player;
    private int temp;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        // Finds Player and sets the healing pad as green
        player = GameObject.Find("JunkoChan");
        obj.GetComponent<MeshRenderer>().material = green;
        temp = 0;
        time = 10f;
    }

    // Update is called once per frame
    // When healing pad is green then the player can heal 10 health. 
    // After healing, the pad turns red for 10 seconds and the player can not heal
    void Update()
    {
        if (temp == 1)
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                obj.GetComponent<MeshRenderer>().material = green;
                temp = 0;
                //Debug.Log("Reset");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "JunkoChan" && temp == 0 && player.GetComponent<JunkochanControl>().Health < player.GetComponent<JunkochanControl>().MaxHealth)
        {
            player.GetComponent<JunkochanControl>().Health += 20;
            obj.GetComponent<MeshRenderer>().material = red;
            temp = 1;
            //Debug.Log("Healed");
            time = 10f;
        }
    }
}
