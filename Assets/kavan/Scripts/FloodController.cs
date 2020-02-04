using UnityEngine;
using UnityEngine.UI;

public class FloodController : MonoBehaviour
{
    [Header("Room Settings")]
    public bool m_introBreach = false;
    public bool m_glassRoom = false;
    public bool m_Breached = false;
    public bool m_hallway = false;

    public TileManager[] m_ConnectedTiles;

    [Header("Flood Settings")]
    public bool m_RoomFlooding = false;
    [Range(0, 1)]
    public float m_PercentFlooded;
    public TileManager m_BreachedTile;
    public GameObject m_BreachGO;
    public GameObject m_BreachOnElements;
    public Sprite[] BreachSprites;
    public Sprite[] RepairSprites;
    private SpriteRenderer BreachSR;

    [Header("Events")]
    public NeoDragonCP.GameEvent StartGameEvent;


    public void RandomBreachSprite()
    {
        int randomNum = Random.Range(0, BreachSprites.Length);
        BreachSR.sprite = BreachSprites[randomNum];
    }
    public void RandomRepairSprite()
    {
        int randomNum = Random.Range(0, RepairSprites.Length);
        BreachSR.sprite = RepairSprites[randomNum];
    }

    public void RepairBreach()
    {
        GameSounds.instance.LeverLatch();

        if (m_introBreach)
        {
            StartGameEvent.Raise();
            m_introBreach = false;

            for (int i = 0; i < m_ConnectedTiles.Length; i++)
            {
                m_ConnectedTiles[i].m_timer = 0.8f;
                m_ConnectedTiles[i].m_isDraining = true;
            }
        }

        //for all repairs
        m_Breached = false;
        RandomRepairSprite();
        for (int i = 0; i < m_ConnectedTiles.Length; i++)
        {
           // if (m_ConnectedTiles[i].m_isBreached)
           // {
                m_ConnectedTiles[i].m_isBreached = false;
                m_ConnectedTiles[i].m_isFlooding = false;
                m_ConnectedTiles[i].m_isDraining = true; //should cascade through them all from that point
           // }
        }

        
       
    }


    private void CheckRoomFloodedStatus()
    {
        int numFloodedTiles = 0;
        for (int i = 0; i < m_ConnectedTiles.Length; i++)
        {
            if (m_ConnectedTiles[i].m_isFlooding)
            {
                numFloodedTiles += 1;
            }
        }
        m_PercentFlooded = numFloodedTiles / m_ConnectedTiles.Length;

        if (m_PercentFlooded > .5f)
        {
            m_RoomFlooding = true;
        }
        else
        {
            m_RoomFlooding = false;
        }
    }

    private void Awake()
    {
        BreachSR = m_BreachGO.GetComponentInChildren<SpriteRenderer>();
        
        if (m_hallway)
        {
            for (int i = 0; i < m_ConnectedTiles.Length; i++)
            {
                m_ConnectedTiles[i].m_isHallway = true;
            }
        }

    }

    private void Update()
    {
        if (m_introBreach)
        {
            for (int i = 0; i < m_ConnectedTiles.Length; i++)
            {
                m_ConnectedTiles[i].m_WaterSlider.value = 5f;
            }
            return; //go no further if intro...
        }

        //A LEAK() has been created in ShipController
        if (m_Breached)
        {
            m_BreachGO.SetActive(true);
            m_BreachOnElements.SetActive(true);
            m_BreachedTile.m_isBreached = true;
        }
        else //Every frame make sure breach elements are off if you're !m_Breached
        {
            m_BreachOnElements.SetActive(false);
        }

        CheckRoomFloodedStatus();
    }
}