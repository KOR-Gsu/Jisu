using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    private Canvas myCanvas;

    public GameObject myContent { get; private set; }
    public GameObject myPrefab;

    virtual public void ShowWindow(Canvas canvas)
    {
        if (myContent == null)
        {
            myCanvas = canvas;
            myContent = Instantiate<GameObject>(myPrefab, myCanvas.transform);
        }
    }

    virtual public void CloseWindow()
    {
        Destroy(gameObject);
    }
}