using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public Transform targetTransform;

    private Canvas canvas;
    private Camera worldCamera;
    private RectTransform rectParent;
    private RectTransform rectDamageText;

    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;
    private Text text;
    private Color alpha;

    public float damage;
    public Color textColor;

    void Start()
    {
        canvas = GameObject.Find("UI").GetComponent<Canvas>();
        worldCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectDamageText = GetComponent<RectTransform>();

        moveSpeed = 2.0f;
        alphaSpeed = 2.0f;
        destroyTime = 2.0f;
        textColor = Color.white;

        text = GetComponent<Text>();
        alpha = textColor;
        text.text = ((int)damage).ToString();
        Invoke("DestroyObject", destroyTime);

        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetTransform.position);
        
        Vector2 localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, worldCamera, out localPos);

        rectDamageText.localPosition = localPos;
    }
    
    void Update()
    {
        //transform.Translate(new Vector3(transform.position.x, moveSpeed * Time.deltaTime, 0));

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
