using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "JunkoChan")
        {
            Debug.Log("Player entered water");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "JunkoChan")
        {
            Debug.Log("Player exited water");
        }
    }

}
