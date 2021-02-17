using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlUIGauge : MonoBehaviour
{
    private Image myContent;
    private Text myPercentage;
    private float lerpSpeed = 20f;
    private float currentFill;
    
    void Start()
    {
        myContent = GetComponent<Image>();
        myPercentage = GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (currentFill != myContent.fillAmount)
            myContent.fillAmount = Mathf.Lerp(myContent.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
    }

    public void Initialize(float rate)
    {
        if(myContent == null)
            myContent = GetComponent<Image>();
        if(myPercentage == null)
            myPercentage = GetComponentInChildren<Text>();

        currentFill = rate;

        myPercentage.text = (currentFill * 100).ToString("F1") + "%";
    }
}