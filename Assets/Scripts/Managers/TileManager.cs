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

    [Header("Flood Settings")]
    public float m_DrainSpeed = 2f;
    public float m_FloodRate = 0;
    public float m_MaxFloodVolume = 10f;
    public Slider m_WaterSlider;
    public GameObject m_WaterHandle;

    [Range(0, 100)]
    [HideInInspector]public int m_PercentFlooded;
    private float m_CurrFloodVolume;

    public void Flood()
    {

        //if done:
        if (m_PercentFlooded >= 100)
        {
            //        //m_tileFlooded = true;
            //        m_WaterHandle.SetActive(false);
            //        PassFlood();
        }
        else
        {

            m_CurrFloodVolume += m_FloodRate * Time.deltaTime;
            m_PercentFlooded = (int)(m_CurrFloodVolume / m_MaxFloodVolume * 100);
            m_WaterSlider.value = m_PercentFlooded;
            //m_tileFlooded = false;
            m_WaterHandle.SetActive(true);

            //pass flood along...
            if (m_CurrFloodVolume > 0)
            {
                PassFlood();
            }
        }

    }

    public void Drain(TileManager drainSource)
    {
        //m_isFlooding = false;
        //m_TileThatPassedDrain = drainSource;

        //if (drainSource == leftTile)
        //{
        //    //Debug.Log(this + "was triggered from LEFT side");
        //    m_NextTileToPushDrainTo = rightTile;
        //}
        //else if (drainSource == rightTile)
        //{
        //    // Debug.Log(this + "was triggered from Right side");
        //    m_NextTileToPushDrainTo = leftTile;
        //}
        //else
        //{
        //    //Debug.Log(this + "'s [room1] drain source:" + drainSource + ", doesn't match left or right, defaulting to drain left");
        //    m_NextTileToPushDrainTo = leftTile; //I think just room one is hit by this
        //}

        //m_isDraining = true;
    }

    void PassFlood()
    {
        //if (m_isBreached)
        //{
        //    //Breach sends flood (with self as source)
        //    //m_NextTileToPushFloodTo = this;
        //    leftTile.Flood();
        //    rightTile.Flood();
        //    return;
        //}

        //if (m_isHallway) //if hallway check self before sending:
        //{
        //    bool isPathClear = CheckDoor(this);
        //    if (isPathClear)
        //    {
        //        m_NextTileToPushFloodTo.Flood(this);
        //    }
        //}
        //else //if room, check next space (hallway):
        //{
        //    if (m_NextTileToPushFloodTo == null)
        //    {
        //        return;
        //    }

        //    bool isPathClear = CheckDoor(m_NextTileToPushFloodTo);
        //    if (isPathClear)
        //    {
        //        m_NextTileToPushFloodTo.Flood(this);
        //    }
        //}
    }

    void PassDrain()
    {
        //if (m_isHallway) //if hallway check self before sending:
        //{
        //    bool isPathClear = CheckDoor(this);
        //    if (isPathClear)
        //    {
        //        m_NextTileToPushDrainTo.Drain(this);
        //    }
        //}
        //else //if room, check next space (hallway):
        //{
        //    if (m_NextTileToPushDrainTo == null)
        //    {
        //        //Debug.LogError(this.transform.parent.parent + "is trying to pass the drain and has null m_DrainSourceDirection, Drain is up against a wall");
        //        return;
        //    }

        //    bool isPathClear = CheckDoor(m_NextTileToPushDrainTo);
        //    if (isPathClear)
        //    {
        //        m_NextTileToPushDrainTo.Drain(this);
        //    }
        //}
    }

    bool CheckDoor(TileManager NextTile)
    {
        //if (NextTile.m_isUnderDoor)
        //{
        //    //Debug.Log(this + " " + NextTile + " isUnderDoor");
        //    if (NextTile.m_ConnectedDoor == null)
        //    {
        //        //Debug.LogError(this + " " + NextTile + "isUnderDoor not assigned, returning false");
        //        return false;
        //    }
        //    else
        //    {
        //        // Debug.Log(this + " " + NextTile + "door assigned");
        //        if (NextTile.m_ConnectedDoor.GetBool("closed"))
        //        {
        //            //Debug.Log(this + " " + NextTile + "door is closed, returning false");
        //            return false;
        //        }
        //        //Debug.Log(this + " " + NextTile + "door is open, returning true");
        //        return true;
        //    }
        //}
        ////Debug.Log(this + " " + NextTile + "is not under a door, returning true");
        return true;
    }

    public void InitiateDrain()
    {
        //m_isBreached = false;
        //m_isFlooding = false;
        //m_isDraining = true;

        //m_NextTileToPushDrainTo = this;

        ////Breach sends Drain (with self as source)
        //leftTile.Drain(this);
        //rightTile.Drain(this);
    }

    private void Update()
    {
        if (m_isBreached)
        {
            Flood();
        }

        //if (m_isDraining)
        //{
        //    m_isFlooding = false;

        //    if (m_timer <= 0)
        //    {
        //        m_WaterHandle.SetActive(false);
        //    }

        //    if (m_timer <= -.5)
        //    {
        //        m_timer = 0;
        //        m_PercentFlooded = 0;
        //        m_WaterSlider.value = m_PercentFlooded;
        //        if(m_NextTileToPushDrainTo != null)
        //        {
        //            PassDrain();
        //        }
        //        m_isDraining = false;
        //    }
        //    else
        //    {
        //        m_timer -= Time.deltaTime * m_DrainSpeed;
        //        m_PercentFlooded = (int)(m_timer / m_TimeToFlood * 100);
        //        m_WaterSlider.value = m_PercentFlooded;
        //        //m_WaterHandle.SetActive(true);

        //        //Send drain to neighbors
        //        if (m_WaterSlider.value % 10 == 5)
        //        {
        //            PassDrain();
        //        }
        //    }
        //}


        ////if door gets closed:
        //if (m_isUnderDoor)
        //{
        //    if (!m_ConnectedDoor.GetBool("closed")) //if the door is open, set status closed status to false (every frame)
        //    {
        //        m_doorClosed = false;
        //    }
        //    if (m_ConnectedDoor.GetBool("closed") && !m_doorClosed) //if door is set to close but I just now find out:
        //    {
        //        m_doorClosed = true;

        //        if (m_isFlooding)
        //        {
        //            m_isFlooding = false;
        //            m_isDraining = true;
        //            m_NextTileToPushFloodTo.Drain(this);
        //        }
        //        //    //PassDrain();
        //        //    //SendDrain(m_FloodPushDirection);
        //        //}
        //    }
        //}
    }
}