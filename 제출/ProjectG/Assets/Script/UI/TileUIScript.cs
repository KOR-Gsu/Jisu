using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TileUIScript : MonoBehaviour
{
    public InputField idInputField;
    public InputField pwInputField;
    public Window registerWindow;
    public Window exitWindow;

    private Canvas myCanvas;
    private Color alertAlpha;
    private Color orignalAlpha;
    private float alphaSpeed = 2.0f;
    private int charMin = 2;
    private int charMax = 8;

    void Start()
    {
        myCanvas = GetComponent<Canvas>();

        orignalAlpha = idInputField.image.color;
    }

    void Update()
    {
        if (idInputField.image.color != orignalAlpha)
        {
            alertAlpha.r = Mathf.Lerp(alertAlpha.r, orignalAlpha.r, Time.deltaTime * alphaSpeed);
            alertAlpha.g = Mathf.Lerp(alertAlpha.g, orignalAlpha.g, Time.deltaTime * alphaSpeed);
            alertAlpha.b = Mathf.Lerp(alertAlpha.b, orignalAlpha.b, Time.deltaTime * alphaSpeed);

            idInputField.image.color = alertAlpha;
        }
        if (pwInputField.image.color != orignalAlpha)
        {
            alertAlpha.r = Mathf.Lerp(alertAlpha.r, orignalAlpha.r, Time.deltaTime * alphaSpeed);
            alertAlpha.g = Mathf.Lerp(alertAlpha.g, orignalAlpha.g, Time.deltaTime * alphaSpeed);
            alertAlpha.b = Mathf.Lerp(alertAlpha.b, orignalAlpha.b, Time.deltaTime * alphaSpeed);

            pwInputField.image.color = alertAlpha;
        }
    }

    public void LogIn()
    {
        if (CheckInputField())
            return;

        LogData currentLogData = new LogData(idInputField.GetComponentInChildren<Text>().text, pwInputField.text);

        if (CheckLogData(currentLogData))
            return;

        DataManager.instance.currentLog = currentLogData;

        DontDestroyOnLoad(DataManager.instance);
        SceneManager.LoadSceneAsync("VillageScene");
    }

    private bool CheckInputField()
    {
        bool idCheck = false;
        bool pwCheck = false;

        if (idInputField.GetComponentInChildren<Text>().text.Length < charMin || idInputField.GetComponentInChildren<Text>().text.Length > charMax)
        {
            AlertInputField(idInputField);
            idCheck = true;
        }
        if (pwInputField.text.Length < charMin || pwInputField.text.Length > charMax)
        {
            AlertInputField(pwInputField);
            pwCheck = true;
        }

        return idCheck || pwCheck;
    }

    private bool CheckLogData(LogData logData)
    {
        LogDataJson logDataJson = DataManager.instance.JsonToData<LogDataJson>(DataManager.instance.logFileName);
        if (logDataJson.IsData(logData.id))
        {
            logDataJson.logDataDictionary.TryGetValue(logData.id, out LogData logtmp);

            if (logData.pw != logtmp.pw)
            {
                AlertInputField(idInputField);
                AlertInputField(pwInputField);
                Debug.Log("Encorrect PW");

                return true;
            }
        }

        return false;
    }

    private void AlertInputField(InputField inputField)
    {
        alertAlpha = Color.red;
        inputField.image.color = alertAlpha;
    }

    public void OpenRegisterWindow()
    {
        registerWindow.ShowWindow(myCanvas);
    }

    public void OpenExitWindow()
    {
        exitWindow.ShowWindow(myCanvas);
    }
}
