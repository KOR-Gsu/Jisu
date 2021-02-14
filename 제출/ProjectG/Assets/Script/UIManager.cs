using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum GAUGE : int
    {
        GAUGE_START,
        GAUGE_HP = 0,
        GAUGE_MP,
        GAUGE_EXP,
        GAUGE_END
    }

    private static UIManager m_instance;
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<UIManager>();

            return m_instance;
        }
    }

    public ControlUIGauge[] Gauges;

    public void UpdateGaugeRate(int index, float rate)
    {
        Gauges[index].Initialize(rate);
    }
}
