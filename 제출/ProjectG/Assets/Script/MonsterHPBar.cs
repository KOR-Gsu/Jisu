using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHPBar : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 offset;

    private Canvas canvas;
    private Camera worldCamera;
    private RectTransform rectParent;
    private RectTransform rectHPBar;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        worldCamera = canvas.worldCamera;
        rectParent = GetComponentInParent<RectTransform>();
        rectHPBar = GetComponent<RectTransform>();
    }
    
    void LateUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + offset);

        if (screenPos.z < 0f)
            screenPos.z *= 1f;

        Vector2 localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, worldCamera, out localPos);

        rectHPBar.localPosition = localPos;
    }
}
