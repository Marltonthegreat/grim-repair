using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloodController : MonoBehaviour
{
    public Slider m_WaterSlider;
    public Image m_RoomOverlay;
    private Color defaultColor;

    [Range(0,1)]
    public float m_PercentFlooded;

    private void Awake()
    {
        defaultColor = m_RoomOverlay.color;
    }

    private void Update()
    {
        //Water Level
        m_WaterSlider.value = m_PercentFlooded;
        //Color Overlay for lack of Oxygen
        m_RoomOverlay.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, m_PercentFlooded);
    }
}
