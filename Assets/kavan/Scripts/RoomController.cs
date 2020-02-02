using UnityEngine;
using UnityEngine.UI;

public class RoomController : MonoBehaviour
{
    [Header("Room Settings")]
    public bool m_introBreach = false;
    public bool m_Breached = false;
    public bool m_RoomFlooding = false;
    public bool m_RoomFlooded = false;
    public bool m_RoomDraining = false;
    public bool m_RoomLocked = false;
    public RoomController[] m_ConnectedRooms;
    public Animator[] m_ConnectedDoors;

    [Header("Flood Settings")]
    public float m_TimeToFlood = 10f;
    private float m_timer;
    public Slider m_WaterSlider;
    [Range(0, 1)]
    public float m_PercentFlooded;
    public GameObject m_BreachGO;

    [Header("Oxygen Overlay")]
    public Image m_RoomOverlay;
    //private Color defaultColor;


    private void Awake()
    {
        //defaultColor = m_RoomOverlay.color;
        m_timer = 0;
    }

    private void CheckLockedStatus()
    {
        if (m_ConnectedDoors.Length != 0)
        {
            int doorCount = m_ConnectedDoors.Length;
            int numClosed = 0;
            for (int i = 0; i < m_ConnectedDoors.Length; i++)
            {
                var isclosed = m_ConnectedDoors[i].GetBool("closed");
                if (isclosed)
                {
                    numClosed += 1;
                }
            }
            if (numClosed == doorCount)
            {
                m_RoomLocked = true;
            }
            else
            {
                m_RoomLocked = false;
            }
        }
    }

    void Overflow()
    {
        if (m_ConnectedDoors.Length != 0)
        {
            for (int i = 0; i < m_ConnectedRooms.Length; i++)
            {
                if (m_ConnectedRooms[i].m_RoomLocked == false)
                {
                    m_ConnectedRooms[i].m_RoomFlooding = true;
                }

            }
        }
    }

    public void Repair()
    {
        if (m_introBreach)
        {
            m_introBreach = false;
            m_timer = 0.8f;
        }

        m_BreachGO.SetActive(false);
        m_RoomDraining = true;
    }







    private void Update()
    {
        if (m_introBreach)
        {
            m_BreachGO.SetActive(true);
            return;
            //don't go any further if this is an intro breach room
        }

        //process draining
        if (m_RoomDraining)
        {
            m_timer -= Time.deltaTime;
            m_PercentFlooded = m_timer / m_TimeToFlood;
            m_WaterSlider.value = m_PercentFlooded;

            if (m_PercentFlooded <= 0)
            {
                m_RoomDraining = false;
            }
        }

        if (m_Breached)
        {
            m_BreachGO.SetActive(true);
        }

        //process flooding
        if (!m_RoomDraining && !m_RoomFlooded && m_RoomFlooding)
        {
            m_timer += Time.deltaTime;
            m_PercentFlooded = m_timer / m_TimeToFlood;
            m_WaterSlider.value = m_PercentFlooded;
            //Color Overlay for lack of Oxygen
            // m_RoomOverlay.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, m_PercentFlooded);
        }

        //Is Room Flooded?
        if (m_PercentFlooded > 1)
        {
            m_RoomFlooded = true;
        }
        else
        {
            m_RoomFlooded = false;
        }


        //Are all connected doors closed?
        CheckLockedStatus();

        //Overflow if not locked down
        if (m_RoomFlooded && !m_RoomLocked)
        {
            Overflow();
            //m_RoomLocked = true;
        }
    }
}
