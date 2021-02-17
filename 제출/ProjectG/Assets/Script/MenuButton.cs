using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    private VerticalLayoutGroup myContent;
    private bool isMenu;

    private void Start()
    {
        myContent = GetComponentInChildren<VerticalLayoutGroup>();
        myContent.gameObject.SetActive(false);

        isMenu = false;
    }

    public void ShowMenu()
    {
        if (!isMenu)
            myContent.gameObject.SetActive(true);
        else
            myContent.gameObject.SetActive(false);

        isMenu = !isMenu;
    }
}
