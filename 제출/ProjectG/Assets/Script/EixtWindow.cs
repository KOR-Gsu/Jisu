using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EixtWindow : Window
{
    public override void ShowWindow()
    {
        base.ShowWindow();

        Time.timeScale = 0;
    }

    public override void CloseWindow()
    {
        Time.timeScale = 1;

        base.CloseWindow();
    }

    public void ExitGame()
    {
        GameManager.instance.SavePlayerInfoToJson();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }
}
