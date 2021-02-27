using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ITEMSORT : int
    {
        Consume,
        Weapon,
        Armor
    }

    public ITEMSORT itemSort;
    public int id;
    public string name;
    public int type;
    public int grade;
    public int info;
    public int price;

    public Sprite itemImage;
}

public class ConsumeItemData : Item
{
    public int value;
}

public class WeaponItemData : Item
{
    public float attackDamageValue;
    public float attackRangeValue;
    public float intvlAttackValue;
}

public class ArmorItemData : Item
{
    public float defenseValue;
}