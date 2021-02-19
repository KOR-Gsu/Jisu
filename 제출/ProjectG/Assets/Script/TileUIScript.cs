using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileUIScript : MonoBehaviour
{
    public InputField idInputField;
    public InputField pwInputField;

    public Window registerWindow;
    public Window exitWindow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OpenRegisterWindow()
    {
        registerWindow.ShowWindow();
    }

    public void OpenExitWindow()
    {
        exitWindow.ShowWindow();
    }
    public void LogIn()
    {

    }
}
