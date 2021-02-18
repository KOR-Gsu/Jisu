using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    private Canvas canvas;

    public GameObject myContent { get; private set; }
    public GameObject myPrefab;

    virtual public void ShowWindow()
    {
        if (myContent == null)
        {
            canvas = UIManager.instance.myCanvas;
            myContent = Instantiate<GameObject>(myPrefab, canvas.transform);
        }
    }

    virtual public void CloseWindow()
    {
        Destroy(gameObject);
    }
}