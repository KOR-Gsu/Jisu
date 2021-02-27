using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : Window
{
    public GridLayoutGroup gridLayoutGroup;
    public int shopType;

    private void Start()
    {
    }

    private void Update()
    {
        
    }

    public void Buy()
    {

    }

    public void Sell()
    {

    }

    public override void ShowWindow(Canvas canvas)
    {
        base.ShowWindow(canvas);

        //SetItem();
    }

    public override void CloseWindow()
    {
        base.CloseWindow();
    }

    private void SetItem()
    {
    }
}
