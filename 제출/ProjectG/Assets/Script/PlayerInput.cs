using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxis = "Vertical";
    public string rotateAxis = "Horizontal";
    public string attackButton = "Attack";
    public string item1Button = "Item1";
    public string item2Button = "Item2";

    public float move { private set; get; }
    public float rotate { private set; get; }
    public bool attack { private set; get; }
    public bool item1 { private set; get; }
    public bool item2 { private set; get; }

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
        item1 = Input.GetButton(item1Button);
        item2 = Input.GetButton(item2Button);
    }
}
