using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Descender : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var speed = 75 / GameConfig.instance.descendTimeInSeconds; // 75 is the height of the ocean in units
        transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
    }
}
