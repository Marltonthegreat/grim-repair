using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public GameObject[] Rooms;

    public float m_GameTime = 90f;
    public FloatVariable m_Timer;
    public float m_PercentDone;

    private void Update()
    {
        m_Timer.currentValue += Time.deltaTime;

        m_PercentDone = m_Timer.currentValue / m_GameTime;


        if (m_Timer.currentValue >= m_GameTime)
        {
            Debug.Log("GAME OVER, time ran out");
        }


    }

}
