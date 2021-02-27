using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindow : Window
{
    private PlayerStat playerStat;
    private Slot[] slots;

    public Text gold;

    [SerializeField]
    private GameObject slotGridLayoutGroup;

    void Start()
    {
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
        slots = slotGridLayoutGroup.GetComponentsInChildren<Slot>();
    }

    void Update()
    {
        gold.text = string.Format("{0:N0}", playerStat.gold);
    }

    public override void ShowWindow(Canvas canvas)
    {
        base.ShowWindow(canvas);
    }

    public override void CloseWindow()
    {
        base.CloseWindow();
    }

    public void AcquireItem(Item newItem, int count = 1)
    {
        if (newItem.itemSort == Item.ITEMSORT.Consume)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.name == newItem.name)
                    {
                        slots[i].UpdateItemCount(count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(newItem);
                return;
            }
        }
    }
}
