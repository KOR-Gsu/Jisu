using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager m_instance;

    private int BlockMove = 1;

    public static InputManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<InputManager>();

            return m_instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
