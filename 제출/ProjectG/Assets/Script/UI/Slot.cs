using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;
    public Image itemImage;
    public int itemCount;

    [SerializeField] private Image itemGradeImage;
    [SerializeField] private Text countText;

    private void SetItemSprite(Sprite newSprite)
    {
        itemImage.sprite = newSprite;

        if (newSprite != null)
            itemImage.gameObject.SetActive(true);
        else
            itemImage.gameObject.SetActive(false);
    }

    private void SetGradeColor(bool active)
    {
        if (active)
        {
            itemGradeImage.gameObject.SetActive(true);
            switch (item.grade)
            {
                case 0:
                    itemGradeImage.color = Color.white;
                    break;
                case 1:
                    itemGradeImage.color = Color.blue;
                    break;
                case 2:
                    itemGradeImage.color = Color.yellow;
                    break;
                default:
                    break;
            }
        }
        else
            itemGradeImage.gameObject.SetActive(false);
    }

    private void SetCountText(bool active)
    {
        if(active)
        {
            countText.gameObject.SetActive(true);
            countText.text = itemCount.ToString();
        }
        else
        {
            countText.gameObject.SetActive(false);
            countText.text = "0";
        }
    }

    public void AddItem(Item newItem, int count = 1)
    {
        item = newItem;
        itemCount = count;
        SetItemSprite(item.itemImage);

        if (item.itemSort == Define.ItemSort.Consume)
        {
            SetGradeColor(false);
            SetCountText(true);
        }
        else
        {
            SetGradeColor(true);
            SetCountText(false);
        }
    }

    public void RemoveItem()
    {
        item = null;
        SetItemSprite(null);
        SetGradeColor(false);
        SetCountText(false);
    }

    public void UpdateItemCount(int count)
    {
        itemCount += count;
        countText.text = itemCount.ToString();

        if(itemCount <= 0)
            RemoveItem();
    }
}
