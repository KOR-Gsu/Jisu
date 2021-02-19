using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if(SceneManager.GetActiveScene().name != "TitleScene")
            GameManager.instance.SavePlayerData();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
