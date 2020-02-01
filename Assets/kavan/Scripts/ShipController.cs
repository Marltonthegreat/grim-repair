using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public GameObject[] Rooms;

    public float m_GameTime = 90f;
    public FloatVariable m_TimeRemaining;
    public FloatVariable m_AirRemaining;

    private void Awake()
    {
        m_TimeRemaining.currentValue = m_GameTime;
    }

    private void Update()
    {
        //Update Time Remaining
        m_TimeRemaining.currentValue -= Time.deltaTime;

        //Update Air Remaining (if it should be a % of time remaining)
        m_AirRemaining.currentValue = m_TimeRemaining.currentValue / m_GameTime;

        if (m_TimeRemaining.currentValue <= 0)
        {
            Debug.Log("GAME OVER, time ran out");
        }



    }

}
