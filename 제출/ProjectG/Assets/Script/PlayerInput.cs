using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxis = "Vertical";
    public string rotateAxis = "Horizontal";
    public string attackButton = "Attack";

    public float move { private set; get; }
    public float rotate { private set; get; }
    public bool attack { private set; get; }
    
    void Update()
    { 
        if(GameManager.instance != null && GameManager.instance.isGameOver)
        {
            move = 0;
            rotate = 0;
            attack = false;

            return;
        }

        move = Input.GetAxis(moveAxis);
        rotate = Input.GetAxis(rotateAxis);
        attack = Input.GetButton(attackButton);
    }
}
