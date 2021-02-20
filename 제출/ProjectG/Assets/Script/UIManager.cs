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

    public enum QUICKSLOT : int
    {
        QUICKSLOT_START,
        QUICKSLOT_ITEM_1 = 0,
        QUICKSLOT_ITEM_2,
        QUICKSLOT_SKILL_1,
        QUICKSLOT_SKILL_2,
        QUICKSLOT_END
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

    public Canvas myCanvas { get; protected set; }

    public Text levelText;
    public ControlUIGauge[] gaugesList;

    public Window infoWindow;
    public Window inventoryWindow;
    public Window exitWindow;

    public Image[] cooldownImageList;

    private void Start()
    {
        myCanvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        
    }

    public void UpdateLevelText(float level)
    {
        levelText.text = ((int)level).ToString();
    }

    public void UpdateGaugeRate(int index, float rate)
    {
        gaugesList[index].Initialize(rate);
    }

    public void OpenCharacterInfo()
    {
        infoWindow.ShowWindow(UIManager.instance.myCanvas);
    }

    public void OpenCharacterInventory()
    {
        inventoryWindow.ShowWindow(UIManager.instance.myCanvas);
    }

    public void OpenExitWindow()
    {
        exitWindow.ShowWindow(UIManager.instance.myCanvas);
    }

    public void StartCooldownCoroutine(int index, float cooldown)
    {
        cooldownImageList[index].gameObject.SetActive(true);
        StartCoroutine(UpdateCooldown(index, cooldown));
    }

    private IEnumerator UpdateCooldown(int index, float cooldown)
    {
        float nowDelayTime = 0;
        while (nowDelayTime < cooldown)
        {
            nowDelayTime += Time.deltaTime;
            cooldownImageList[index].fillAmount = (1.0f - nowDelayTime / cooldown);

            yield return new WaitForFixedUpdate();
        }
    }
}
