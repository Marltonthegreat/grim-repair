using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public GameObject[] Rooms;

    public float m_GameTime = 90f;
    public FloatVariable m_TimeRemaining;
    public FloatVariable m_AirRemaining;
    public bool m_CreateLeak = false;

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

        if (m_CreateLeak)
        {
            Leak();
        }
    }

    void Leak()
    {
        var num = Random.Range(0, Rooms.Length);
        var isCurRoomFlooding = Rooms[num].GetComponent<RoomController>().m_RoomFlooding;

        if (isCurRoomFlooding == true)
        {
            Debug.Log("the current room should be flooding already : " + isCurRoomFlooding);
            return;
            //jump out and auto try again since Create leak is still true
        }

        Rooms[num].GetComponent<RoomController>().m_RoomFlooding = true;

        m_CreateLeak = false;
    }
}
