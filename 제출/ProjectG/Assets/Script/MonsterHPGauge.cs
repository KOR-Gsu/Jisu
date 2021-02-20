using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPGauge : MonoBehaviour
{
    public Image myContent;
    public Text myPercentage;

    [HideInInspector] public Transform targetTransform;

    private Camera worldCamera;
    private RectTransform rectParent;
    private RectTransform rectHPBar;
    private Vector2 offSet;
    private float lerpSpeed = 10f;
    private float currentFill;

    void Awake()
    {
        worldCamera = UIManager.instance.myCanvas.worldCamera;
        rectParent = UIManager.instance.myCanvas.GetComponent<RectTransform>();
        rectHPBar = GetComponent<RectTransform>();
        offSet = new Vector2(0f, 30f);
    }
    
    void Update()
    {
        if (currentFill != myContent.fillAmount)
            myContent.fillAmount = Mathf.Lerp(myContent.fillAmount, currentFill, Time.deltaTime * lerpSpeed);

        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetTransform.position);
        Vector2 localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, worldCamera, out localPos);

        rectHPBar.localPosition = localPos + offSet;
    }

    public void Initialize(float rate)
    {
        currentFill = rate;

        myPercentage.text = (currentFill * 100).ToString("F1") + "%";
    }
}
