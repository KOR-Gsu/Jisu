using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxis = "Vertical";
    public string rotateAxis = "Horizontal";
    public string attackAxis = "Normal Attack";

    public float move { private set; get; }
    public float rotate { private set; get; }
    public float attack { private set; get; }
    
    void Update()
    { 
        move = Input.GetAxis(moveAxis);
        rotate = Input.GetAxis(rotateAxis);
        attack = Input.GetAxis(attackAxis);
    }
}
