using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager _instance;
    public static ItemManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<ItemManager>();

            return _instance;
        }
    }

    public List<ConsumeItemData> consumeItemDataList = new List<ConsumeItemData>();
    public List<WeaponItemData> weaponItemDataList = new List<WeaponItemData>();
    public List<ArmorItemData> armorItemDataList = new List<ArmorItemData>();

    private void Start()
    {
        ItemDataJson itemDataJson = DataManager.instance.JsonToData<ItemDataJson>(DataManager.instance.itemDataFileName);

        for(int i = 0; i < itemDataJson.consumeItemDataDictionary.Keys.Count; i++)
            consumeItemDataList.Add(itemDataJson.consumeItemDataDictionary[i]);

        for (int i = 0; i < itemDataJson.weaponItemDataDictionary.Keys.Count; i++)
            weaponItemDataList.Add(itemDataJson.weaponItemDataDictionary[i]);

        for (int i = 0; i < itemDataJson.armorItemDataDictionary.Keys.Count; i++)
            armorItemDataList.Add(itemDataJson.armorItemDataDictionary[i]);
    }

    public Item FindItem(Define.ItemSort sort, int id)
    {
        switch (sort)
        {
            case Define.ItemSort.Consume:
                return consumeItemDataList[id];
            case Define.ItemSort.Weapon:
                return weaponItemDataList[id];
            case Define.ItemSort.Armor:
                return armorItemDataList[id];
            default:
                return null;
        }
    }
}
