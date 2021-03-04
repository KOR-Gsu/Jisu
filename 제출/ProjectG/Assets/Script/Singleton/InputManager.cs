using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<InputManager>();

            return _instance;
        }
    }
    public Action keyAction;
    public Action<Define.Mouse, Define.MouseEvent> mouseAction;

    private bool isMouse0Pressed = false;
    private bool isMouse1Pressed = false;
    private bool isMouse2Pressed = false;

    private void Update()
    {
        if (Input.anyKey && keyAction != null)
            keyAction.Invoke();

        if (mouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                mouseAction.Invoke(Define.Mouse.Mouse_0, Define.MouseEvent.Press);
                isMouse0Pressed = true;
            }
            else
            {
                if (isMouse0Pressed)
                    mouseAction.Invoke(Define.Mouse.Mouse_0, Define.MouseEvent.Click);

                isMouse0Pressed = false;
            }

            if (Input.GetMouseButton(1))
            {
                mouseAction.Invoke(Define.Mouse.Mouse_1, Define.MouseEvent.Press);
                isMouse1Pressed = true;
            }
            else
            {
                if (isMouse1Pressed)
                    mouseAction.Invoke(Define.Mouse.Mouse_1, Define.MouseEvent.Click);

                isMouse1Pressed = false;
            }

            if (Input.GetMouseButton(2))
            {
                mouseAction.Invoke(Define.Mouse.Mouse_2, Define.MouseEvent.Press);
                isMouse2Pressed = true;
            }
            else
            {
                if (isMouse2Pressed)
                    mouseAction.Invoke(Define.Mouse.Mouse_2, Define.MouseEvent.Click);

                isMouse2Pressed = false;
            }
        }
    }
}
