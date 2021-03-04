using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Mouse
    {
        Mouse_0,
        Mouse_1,
        Mouse_2
    }
    public enum CursorType
    {
        None,
        Default,
        Attack
    }
    public enum MouseEvent
    {
        Press,
        Click
    }
    public enum PlayerState
    {
        Die,
        Idle,
        Moving,
        Attack,
        Skill
    }

    public enum EnemyType
    {
        warrior,
        archer
    }

    public enum EnemyState
    {
        Die,
        Idle,
        Moving,
        Attack
    }

    public enum Gauge
    {
        HP,
        MP,
        EXP
    }

    public enum Menu
    {
        Info,
        Inventory,
        Eixt
    }
    public enum QuckSlot
    {
        Item_1,
        Item_2,
        Skill_1,
        Skill_2
    }
    public enum ItemSort
    {
        Consume,
        Weapon,
        Armor
    }
    public enum Stat
    {
        LV,
        HP,
        MP,
        EXP,
        ATK,
        DEF,
        RNG,
        SPD,
        APS
    }
}
