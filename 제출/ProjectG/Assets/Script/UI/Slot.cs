using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;
    public Image itemImage;
    public int itemCount;

    [SerializeField]
    private Image itemGradeImage;
    [SerializeField]
    private Text countText;

    private void SetColor(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        item.itemImage = newItem.itemImage;

        if(item.itemSort == Item.ITEMSORT.Consume)
        {
            countText.gameObject.SetActive(true);
            countText.text = itemCount.ToString();
        }
        else
        {
            countText.gameObject.SetActive(false);
            countText.text = "0";
        }

        SetColor(1.0f);
    }

    public void RemoveItem()
    {
        item = null;
        itemImage.sprite = null;
        SetColor(0f);

        countText.gameObject.SetActive(false);
        countText.text = "0";
    }

    public void UpdateItemCount(int count)
    {
        itemCount += count;
        countText.text = itemCount.ToString();

        if(itemCount <= 0)
            RemoveItem();
    }
}
