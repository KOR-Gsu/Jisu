using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHPBar : MonoBehaviour
{
    public Transform targetTransform;
    public Vector2 offSet;

    private Canvas canvas;
    private Camera worldCamera;
    private RectTransform rectParent;
    private RectTransform rectHPBar;

    void Start()
    {
        canvas = GameObject.Find("UI").GetComponent<Canvas>();
        worldCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHPBar = GetComponent<RectTransform>();
    }
    
    void LateUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetTransform.position);

        if (screenPos.z < 0f)
            screenPos.z *= 1f;

        Vector2 localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, worldCamera, out localPos);

        rectHPBar.localPosition = localPos + offSet;
    }
}
