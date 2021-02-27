using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public LayerMask targetLayer;
    public Window shopWindowScript;

    public void OpenShop()
    {
        shopWindowScript.ShowWindow(UIManager.instance.myCanvas);
    }
}
