using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private string targetingBtn = "Targeting";
    private string attackBtn = "Attack";
    private string item1Btn = "Item1";
    private string item2Btn = "Item2";
    private string skill1Btn = "Skill1";
    private string skill2Btn = "Skill2";
    private string test1Btn = "Test1";
    private string test2Btn = "Test2";

    public bool targeting { private set; get; }
    public bool attack { private set; get; }
    public bool item1 { private set; get; }
    public bool item2 { private set; get; }
    public bool skill1 { private set; get; }
    public bool skill2 { private set; get; }
    public bool test1 { private set; get; }
    public bool test2 { private set; get; }

    void Update()
    { 
        if(GameManager.instance != null && GameManager.instance.isGameOver)
        {
            targeting = false;
            attack = false;
            item1 = false;
            item2 = false;
            skill1 = false;
            skill2 = false;
            test1 = false;
            test2 = false;

            return;
        }

        targeting = Input.GetButtonDown(targetingBtn);
        attack = Input.GetButtonDown(attackBtn);
        item1 = Input.GetButtonDown(item1Btn);
        item2 = Input.GetButtonDown(item2Btn);
        skill1 = Input.GetButtonDown(skill1Btn);
        skill2 = Input.GetButtonDown(skill2Btn);
        test1 = Input.GetButtonDown(test1Btn);
        test2 = Input.GetButtonDown(test2Btn);
    }
}
