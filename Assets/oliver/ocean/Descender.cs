using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Descender : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // 750 is the height of the ocean in units
        var speed = 750 / GameConfig.instance.descendTimeInSeconds; 
        transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
    }
}
