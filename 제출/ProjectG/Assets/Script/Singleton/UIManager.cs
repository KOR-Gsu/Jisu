using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<UIManager>();

            return _instance;
        }
    }

    public Canvas myCanvas { get; protected set; }

    [SerializeField] private Text levelText;
    [SerializeField] private GaugeControl[] gaugesList;
    [SerializeField] private QuickSlot[] quickSlotList;

    private void Start()
    {
        myCanvas = GetComponent<Canvas>();
    }

    public void UpdateLevelText(float level)
    {
        levelText.text = ((int)level).ToString();
    }

    public void UpdateGaugeRate(Define.Gauge index, float rate)
    {
        gaugesList[(int)index].Initialize(rate);
    }

    public bool UseQuickSlot(Define.QuckSlot index)
    {
        if (!quickSlotList[(int)index].isCooldown)
        {
            quickSlotList[(int)index].StartCooldown();
            return true;
        }

        return false;
    }
}
