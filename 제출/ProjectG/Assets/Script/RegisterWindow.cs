using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterWindow : Window
{
    public InputField idInputField;
    public InputField pwInputField;

    public override void ShowWindow()
    {
        base.ShowWindow();
    }

    public override void CloseWindow()
    {
        base.CloseWindow();
    }

    public void RegisterPlayer()
    {
        LogData logData = new LogData(idInputField.GetComponent<Text>().text, pwInputField.text);

        LogDataJson logDataFile = DataManager.instance.JsonToData<LogDataJson>(DataManager.instance.logFileName);
        if(logDataFile.IsData(logData.id))
        {
            idInputField.image.color = Color.red;
        }

        logDataFile.Add(logData);
        DataManager.instance.DataToJson<LogDataJson>(DataManager.instance.logFileName, logDataFile);
    }

    public void ResetInputfieldColor()
    {
        idInputField.image.color = Color.white;
    }
}
