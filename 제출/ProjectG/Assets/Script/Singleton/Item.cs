using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public Define.ItemSort itemSort;
    public int id;
    public string name;
    public int type;
    public int grade;
    public string info;
    public int price;

    public Sprite itemImage;

    public void SetImage()
    {
        itemImage = ResourceManager.instance.Load<Sprite>(DataManager.instance.spritePath + "/" + name);
    }
}

public class ConsumeItemData : Item
{
    public int value;

    ConsumeItemData()
    {
        itemSort = Define.ItemSort.Consume;
    }
}

public class WeaponItemData : Item
{
    public float attackDamageValue;
    public float attackRangeValue;
    public float intvlAttackValue;

    WeaponItemData()
    {
        itemSort = Define.ItemSort.Weapon;
    }
}

public class ArmorItemData : Item
{
    public float defenseValue;

    ArmorItemData()
    {
        itemSort = Define.ItemSort.Armor;
    }
}