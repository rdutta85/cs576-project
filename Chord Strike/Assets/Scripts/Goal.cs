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
    public int temp;
    // Start is called before the first frame update
    void Start()
    {
        obj.GetComponent<MeshRenderer>().material = red;
        temp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (1 == 1) { // Change condition to boss is killed or amount of enemies defeated
            obj.GetComponent<MeshRenderer>().material = green;
            temp = 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "JunkoChan")
        {
            if (temp == 1) {
                SceneManager.LoadScene("Scenes/Menu");
                temp = 0;
            }
        }
    }
}
