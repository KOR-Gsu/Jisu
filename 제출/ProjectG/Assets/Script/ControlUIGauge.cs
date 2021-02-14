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
    }

    public void Initialize(float rate)
    {
        currentFill = rate;

        if (currentFill != myContent.fillAmount)
            myContent.fillAmount = Mathf.Lerp(myContent.fillAmount, currentFill, Time.deltaTime * lerpSpeed);

        myPercentage.text = ((int)(currentFill * 100)).ToString() + "%";
    }
}