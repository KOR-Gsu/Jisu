using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlUIGauge : MonoBehaviour
{
    public Image myContent;
    public Text myPercentage;

    private float lerpSpeed = 20f;
    private float currentFill;

    void Update()
    {
        if (currentFill != myContent.fillAmount)
            myContent.fillAmount = Mathf.Lerp(myContent.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
    }

    public void Initialize(float rate)
    {
        currentFill = rate;

        myPercentage.text = (currentFill * 100).ToString("F1") + "%";
    }
}