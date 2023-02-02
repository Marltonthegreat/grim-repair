using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public bool m_isBreached = false;
    public bool m_isFlooding = false;
    public bool m_isDraining = false;
    public bool m_isHallway = false;
    public bool DisplayDebug = false;
    private bool m_CheckedEqualize = false;

    [Header("Door Settings")]
    public bool m_isUnderDoor = false;
    public Animator m_ConnectedDoor;
    public bool m_doorClosed = true;

    public TileManager leftTile;
    public TileManager rightTile;

    [Header("Flood Settings")]
    public float m_DrainSpeed = .5f;
    public float m_FloodRate = 0;
    public float m_MaxFloodVolume = 10f;
    public Slider m_WaterSlider;
    public GameObject m_WaterHandle;

    public float m_PercentFlooded { get { return Mathf.Clamp(m_CurrFloodVolume / m_MaxFloodVolume * 100, 0, 100); } }
    private float m_CurrFloodVolume;

    public void Flood()
    {
        if (m_PercentFlooded >= 100) return;
        else
        {
            m_CurrFloodVolume += m_FloodRate * Time.deltaTime;
        }

    }

    public void Drain()
    {
        if (m_PercentFlooded <= 0) return;
        else
        {
            m_isFlooding = false;
            m_CurrFloodVolume -= m_DrainSpeed * Time.deltaTime;
        }

        #region Old Code
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
        #endregion
    }

    void EqualizeWater()
    {
        if (!CheckDoor(this)) return;
        //if (m_CheckedEqualize) return; else m_CheckedEqualize = true;

        bool leftValid = leftTile && CheckDoor(leftTile);
        bool rightValid = rightTile && CheckDoor(rightTile);
        float leftDiff = 0;
        float rightDiff = 0;

        if (leftValid)
        {
            leftDiff = m_CurrFloodVolume - leftTile.m_CurrFloodVolume;
            leftDiff = (leftDiff) * Time.fixedDeltaTime;
            leftTile.m_CurrFloodVolume += leftDiff;
            if (m_isFlooding) leftTile.m_isFlooding = true;
        }
        if (rightValid)
        {
            rightDiff = m_CurrFloodVolume - rightTile.m_CurrFloodVolume;
            rightDiff = (rightDiff) * Time.fixedDeltaTime;
            rightTile.m_CurrFloodVolume += rightDiff;
            if (m_isFlooding) rightTile.m_isFlooding = true;
        }

        m_CurrFloodVolume -= leftDiff + rightDiff;

        //if (leftValid) leftTile.EqualizeWater();
        //if (rightValid) rightTile.EqualizeWater();
        #region Old Code

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
        #endregion
    }

    bool CheckDoor(TileManager nextTile)
    {
        if (nextTile.m_isUnderDoor)
        {
            if (nextTile.m_ConnectedDoor == null)
            {
                return false;
            }
            else
            {
                if (nextTile.m_ConnectedDoor.GetBool("closed"))
                {
                    return false;
                }
                return true;
            }
        }
        return true;
    }

    private void DisplayWater()
    {
        m_WaterSlider.value = m_PercentFlooded;
        if (m_PercentFlooded > 0 && m_PercentFlooded <= 100) m_WaterHandle.SetActive(true); else m_WaterHandle.SetActive(false);
        if (DisplayDebug) Debug.Log($"Water Percentage: {m_PercentFlooded}");
    }

    private void Update()
    {
        //m_CheckedEqualize = false;


        if (m_isBreached)
        {
            Flood();
        }

        if (m_PercentFlooded > 0)
            EqualizeWater();

        if (m_isDraining)
        {
            Drain();
        }


        DisplayWater();

        #region Old Code
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
        #endregion
    }
}