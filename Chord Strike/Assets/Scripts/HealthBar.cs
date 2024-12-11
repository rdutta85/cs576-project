using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{

    public Slider slider;
    private Camera camera;

    // Update is called once per frame
    void Update()
    {
        camera = Camera.main;
        transform.rotation = camera.transform.rotation;
    }

    public void UpdateHealthBar(float curr, float max){
        slider.value = curr/max;
    }
}
