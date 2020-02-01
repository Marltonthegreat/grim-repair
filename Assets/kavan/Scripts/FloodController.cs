using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class FloodController : MonoBehaviour
{
    public Slider m_WaterSlider;
    public Image m_RoomOverlay;
    private Color defaultColor;

    [Range(0,1)]
    public float m_PercentFlooded;

    public bool m_RoomFlooding = false;
    public bool m_RoomFlooded = false;

    public float m_TimeToFlood = 10f;
    private float m_timer;

    private void Awake()
    {
        defaultColor = m_RoomOverlay.color;
    }

    private void Update()
    {
        if (m_RoomFlooding && !m_RoomFlooded)
        {
            m_timer += Time.deltaTime;

            m_PercentFlooded = m_timer / m_TimeToFlood;

            //Water Level
            m_WaterSlider.value = m_PercentFlooded;
            //Color Overlay for lack of Oxygen
            m_RoomOverlay.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, m_PercentFlooded);

            if (m_PercentFlooded > .99)
            {
                m_RoomFlooded = true;
            }
        }
    }
}
