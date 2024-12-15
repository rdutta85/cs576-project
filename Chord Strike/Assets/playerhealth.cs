using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerhealth : MonoBehaviour
{
    private JunkochanControl junko;
    public Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = junko.Health;

        // color red if health is less than 25
        if (junko.Health < 25)
        {
            healthSlider.fillRect.GetComponent<Image>().color = Color.red;
        }
        else if (junko.Health < 50)
        {
            healthSlider.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            healthSlider.fillRect.GetComponent<Image>().color = Color.green;
        }

    }
}
