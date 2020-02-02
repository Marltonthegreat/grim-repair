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

    public void Leak()
    {
        var num = Random.Range(0, Rooms.Length);
        var isCurRoomFlooding = Rooms[num].GetComponent<RoomController>().m_RoomFlooding;
        var isCurRoomIntro = Rooms[num].GetComponent<RoomController>().m_introBreach;

        if (isCurRoomFlooding == true || isCurRoomIntro == true)
        {
            Debug.Log("the current room should be flooding already : " + isCurRoomFlooding);
            return;
            //jump out and auto try again since CreateLeak is still true
        }

        //Breach
        //RandomRotation
        Vector3 euler = Rooms[num].GetComponent<RoomController>().m_BreachGO.transform.eulerAngles;
        euler.z = Random.Range(0f, 360f);
        Rooms[num].GetComponent<RoomController>().m_BreachGO.transform.transform.eulerAngles = euler;
        //RandomScale
        var randomScale = Random.Range(.5f, 1f);
        Rooms[num].GetComponent<RoomController>().m_BreachGO.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        Rooms[num].GetComponent<RoomController>().m_Breached = true;

        //Start Flooding
        Rooms[num].GetComponent<RoomController>().m_RoomFlooding = true;
        
        //Stop Trying to CreateLeaks
        m_CreateLeak = false;
    }
}
