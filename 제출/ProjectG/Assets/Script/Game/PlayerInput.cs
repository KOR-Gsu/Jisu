using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public string moveAxis = "Vertical";
    [HideInInspector] public string rotateAxis = "Horizontal";
    [HideInInspector] public string targetingBtn = "Targeting";
    [HideInInspector] public string attackBtn = "Attack";
    [HideInInspector] public string item1Btn = "Item1";
    [HideInInspector] public string item2Btn = "Item2";
    [HideInInspector] public string skill1Btn = "Skill1";
    [HideInInspector] public string skill2Btn = "Skill2";

    public float move { private set; get; }
    public float rotate { private set; get; }
    public bool targeting { private set; get; }
    public bool attack { private set; get; }
    public bool item1 { private set; get; }
    public bool item2 { private set; get; }
    public bool skill1 { private set; get; }
    public bool skill2 { private set; get; }

    void Update()
    { 
        if(GameManager.instance != null && GameManager.instance.isGameOver)
        {
            move = 0;
            rotate = 0;
            targeting = false;
            attack = false;
            item1 = false;
            item2 = false;
            skill1 = false;
            skill2 = false;

            return;
        }

        move = Input.GetAxis(moveAxis);
        rotate = Input.GetAxis(rotateAxis);
        targeting = Input.GetButtonDown(targetingBtn);
        attack = Input.GetButtonDown(attackBtn);
        item1 = Input.GetButtonDown(item1Btn);
        item2 = Input.GetButtonDown(item2Btn);
        skill1 = Input.GetButtonDown(skill1Btn);
        skill2 = Input.GetButtonDown(skill2Btn);
    }
}
