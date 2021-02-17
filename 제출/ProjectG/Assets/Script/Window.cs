using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    private Canvas canvas;
    private GameObject myContent;

    public GameObject myPrefab;

    virtual public void ShowWindow()
    {
        canvas = UIManager.instance.myCanvas;
        myContent = Instantiate<GameObject>(myPrefab, canvas.transform);
    }

    virtual public void CloseWindow()
    {
        Destroy(gameObject);
    }
}
