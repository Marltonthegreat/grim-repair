using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public GameObject[] Rooms;
    public bool m_CreateLeak = false;
    public float m_LeakPercentage;

    private void Update()
    {
        ShipStatus();

        if (m_CreateLeak)
        {
            Leak();
        }
    }

    private void ShipStatus()
    {
        float roomCount = Rooms.Length;
        float roomsFlooding = 0;

        for (int i = 0; i < Rooms.Length; i++)
        {
            if (Rooms[i].GetComponent<RoomController>().m_RoomFlooding)
            {
                roomsFlooding += 1;
            }
        }
        m_LeakPercentage = roomsFlooding / roomCount;
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

        if (Rooms[num].GetComponent<RoomController>().m_glassRoom)
        {
            Rooms[num].GetComponent<RoomController>().m_BreachGO.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        Rooms[num].GetComponent<RoomController>().RandomBreachSprite();

        //CALL LEAK SOUND
        GameSounds.instance.PipeBurst();

        Rooms[num].GetComponent<RoomController>().m_Breached = true;
        
        //Stop Trying to CreateLeaks
        m_CreateLeak = false;
    }
}
