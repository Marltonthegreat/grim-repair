using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public bool m_isBreached = false;
    public bool m_isFlooding = false;
    public bool m_isDraining = false;
    public bool m_isHallway = false;

    [Header("Door Settings")]
    public bool m_isUnderDoor = false;
    public Animator m_ConnectedDoor;
    public bool m_doorClosed = true;

    public TileManager leftTile;
    public TileManager rightTile;
    public TileManager m_FloodSourceDirection; //opposite side of where the flood call was passed from
    public TileManager m_DrainSourceDirection;

    [Header("Flood Settings")]
    public float m_DrainSpeed = 2f;
    public float m_TimeToFlood = 10f;
    public float m_timer = 0f;
    public Slider m_WaterSlider;
    public GameObject m_WaterHandle;
    [Range(0, 100)]
    public int m_PercentFlooded;

    private void Update()
    {
        if (m_isBreached)
        {
            m_isFlooding = true;
        }

        //if door gets closed:
        if (m_isUnderDoor)
        {
            if (m_ConnectedDoor == null)
            {
                Debug.LogError(this + "isUnderDoor not assigned, returning");
                return;
            }
            else
            {
                if (!m_ConnectedDoor.GetBool("closed")) //if the door is open, set status closed status to false (every frame)
                {
                    m_doorClosed = false;
                }
                if (m_ConnectedDoor.GetBool("closed") && !m_doorClosed) //if door is set to close but I just now find out:
                {
                    m_doorClosed = true;

                    m_isFlooding = false;
                    m_isDraining = true; //will cascade through isDraining function below

                    if (m_FloodSourceDirection == null)
                    {
                        if (leftTile != null)
                        {
                            SendDrain(leftTile);
                        }
                        if (rightTile != null)
                        {
                            SendDrain(rightTile);
                        }
                    }
                    else
                    {
                        SendDrain(m_FloodSourceDirection);
                    }
                }
            }
        }

        //ACT ON STATUS
        if (m_isFlooding)
        {
            //if done:
            if (m_PercentFlooded >= 100)
            {
                //m_tileFlooded = true;
                m_WaterHandle.SetActive(false);
                PassAlongFlood();
            }
            else
            {
                m_timer += Time.deltaTime;
                m_PercentFlooded = (int)(m_timer / m_TimeToFlood * 100);
                m_WaterSlider.value = m_PercentFlooded;
                //m_tileFlooded = false;
                m_WaterHandle.SetActive(true);
                
                //pass flood along...
                if (m_WaterSlider.value % 10 == 5)
                {
                    //Debug.Log(this + " is sending out the flooding call: " + m_WaterSlider.value + " : " + m_WaterSlider.value % 20);
                    PassAlongFlood();
                }
            }
        }

        if (m_isDraining)
        {
            //if done:
            if (m_timer <= 0)
            {
                m_isDraining = false;
                m_PercentFlooded = 0;
                m_WaterSlider.value = m_PercentFlooded;
                m_WaterHandle.SetActive(false);
            }
            else
            {
                m_timer -= Time.deltaTime * m_DrainSpeed;
                m_PercentFlooded = (int)(m_timer / m_TimeToFlood * 100);
                m_WaterSlider.value = m_PercentFlooded;
                m_WaterHandle.SetActive(true);

                //Send drain to neighbors
                if (m_WaterSlider.value % 10 == 5)
                {
                    if (leftTile != null)
                    {
                        SendDrain(leftTile);
                    }
                    if (rightTile != null)
                    {
                        SendDrain(rightTile);
                    }
                }
            }
        }
    }
    


    void PassAlongFlood()
    {
        if (m_isBreached)
        {
            if (leftTile != null)
            {
                //Debug.Log("breach passing flood left");
                SendFlood(leftTile);
            }
            if (rightTile != null)
            {
                //Debug.Log("breach passing flood right");
                SendFlood(rightTile);
            }
        }
        else
        {
            if (m_isHallway) //if hallway check self before sending:
            {
                bool isPathClear = CheckDoor(this);
                if (isPathClear)
                {
                    if (m_FloodSourceDirection != null)
                    {
                        SendFlood(m_FloodSourceDirection);
                    } 
                }
            }
            else //if room, check next space (hallway):
            {
                if (m_FloodSourceDirection != null)
                {
                    bool isPathClear = CheckDoor(m_FloodSourceDirection);
                    if (isPathClear)
                    {
                        //SendFlood(m_FloodSourceDirection);
                        if (leftTile != null)
                        {
                            //Debug.Log("breach passing flood left");
                            SendFlood(leftTile);
                        }
                        if (rightTile != null)
                        {
                            //Debug.Log("breach passing flood right");
                            SendFlood(rightTile);
                        }
                    }
                }               
            }
        }
    }

    bool CheckDoor(TileManager NextTile)
    {
        if (NextTile.m_isUnderDoor)
        {
            //Debug.Log(this + " " + NextTile + " isUnderDoor");
            if (NextTile.m_ConnectedDoor == null)
            {
                //Debug.LogError(this + " " + NextTile + "isUnderDoor not assigned, returning false");
                return false;
            }
            else
            {
                // Debug.Log(this + " " + NextTile + "door assigned");
                if (NextTile.m_ConnectedDoor.GetBool("closed"))
                {
                    //Debug.Log(this + " " + NextTile + "door is closed, returning false");
                    return false;
                }
                //Debug.Log(this + " " + NextTile + "door is open, returning true");
                return true;
            }
        }
        //Debug.Log(this + " " + NextTile + "is not under a door, returning true");
        return true;
    }
    void SendFlood(TileManager NextTile)
    {
        if (NextTile != null)
        {
            //Debug.Log(this + " " + NextTile + "is not null");
            //bool isPathClear = CheckDoor(NextTile);
           // if (isPathClear)
           // {
                //Debug.Log(this + " " + NextTile + "is being flooded");
                NextTile.Flood(this);
           // }
        }
    }
    void SendDrain(TileManager NextTile)
    {
        if (NextTile != null)
        {
            //Debug.Log(this + " " + NextTile + "is not null");
            bool isPathClear = CheckDoor(NextTile);
            if (isPathClear)
            {
                //Debug.Log(this + " " + NextTile + "is being flooded");
                NextTile.Drain(this);
            }
        }
    }


    //RECEIVE
    public void Flood(TileManager floodSource)
    {
        if (floodSource == leftTile)
        {
            //Debug.Log(this + "was triggered from LEFT side");
            m_FloodSourceDirection = rightTile;
        }
        else if (floodSource == rightTile)
        {
            //Debug.Log(this + "was triggered from Right side");
            m_FloodSourceDirection = leftTile;
        }
        else
        {
            Debug.Log("source doesn't match left or right, defaulting to flood left");
            m_FloodSourceDirection = leftTile;
        }

        m_isDraining = false;
        m_isFlooding = true;
    }
    public void Drain(TileManager drainSource)
    {
        m_DrainSourceDirection = drainSource;
        m_isFlooding = false;
        m_isDraining = true;
    }
}